<script>

    import { onMount } from "svelte";
    import { getTags } from "$lib/services/tagService.js";
    import { getPosts } from "$lib/services/postService.js";
    import Post from "./Post.svelte";
    import { fade, scale } from 'svelte/transition';
    import { browser } from '$app/environment';
    import { refreshPosts } from '$lib/stores/postStore.js';    
    import { createPost } from "$lib/services/newPostService.js";
    import { goto } from "$app/navigation";
    import { programmingLanguages, languageCategories, getLanguageIcon, getLanguageLabel } from "$lib/data/languages.js";
    import { getCurrentUser } from "$lib/services/loginService.js";
    
    function fadeScale(node, params) {
        params = params || {};
        var delay = params.delay || 0;
        var duration = params.duration || 350;
        return {
            delay: delay,
            duration: duration,
            css: function(t) {
                var fadeCss = fade(node, params).css(t);
                var scaleCss = scale(node, Object.assign({}, params, { start: 0.8 })).css(t);
                return fadeCss + scaleCss;
            }
        };
    }

    let tags = [];
    let tagsError = null;
    let allPosts = []; // Store all posts from API
    let posts = []; // Filtered and sorted posts for display
    let postsError = null;
    let postsLoading = true;
    let currentUser = null;

    // Filter and sort state
    let selectedLanguage = '';
    let selectedLanguageCategory = 'all';
    let selectedStatus = '';
    let sortOrder = 'newest'; // 'newest', 'oldest', 'upvotes', 'downvotes'
    let searchQuery = '';

    // Langages filtrés par catégorie
    $: filteredLanguagesForDropdown = selectedLanguageCategory === 'all' 
        ? programmingLanguages 
        : programmingLanguages.filter(lang => lang.category === selectedLanguageCategory);

    // Get unique statuses for filter options
    $: availableStatuses = [...new Set(allPosts.map(post => post.status).filter(Boolean))];

    // Apply filters and sorting whenever dependencies change
    $: {
        let filteredPosts = allPosts.slice();

        // Apply search filter
        if (searchQuery.trim()) {
            filteredPosts = filteredPosts.filter(post => 
                post.title.toLowerCase().includes(searchQuery.toLowerCase()) ||
                post.content.toLowerCase().includes(searchQuery.toLowerCase())
            );
        }

        // Apply language filter
        if (selectedLanguage) {
            filteredPosts = filteredPosts.filter(post => post.language === selectedLanguage);
        }

        // Apply status filter
        if (selectedStatus) {
            filteredPosts = filteredPosts.filter(post => post.status === selectedStatus);
        }

        // Apply sorting
        filteredPosts.sort((a, b) => {
            switch (sortOrder) {
                case 'newest':
                    const dateA = new Date(a.createdAt);
                    const dateB = new Date(b.createdAt);
                    return dateB - dateA;
                case 'oldest':
                    const dateA2 = new Date(a.createdAt);
                    const dateB2 = new Date(b.createdAt);
                    return dateA2 - dateB2;
                case 'upvotes':
                    return b.upvotes - a.upvotes;
                case 'downvotes':
                    return b.downvotes - a.downvotes;
                default:
                    return 0;
            }
        });

        posts = filteredPosts;
    }

    async function loadPosts() {
        try {
            const result = await getPosts();
            allPosts = Array.isArray(result) ? result : (Array.isArray(result?.$values) ? result.$values : []);
            console.log('Posts refreshed:', allPosts);
        } catch (error) {
            postsError = error;
            console.error("Error fetching posts:", error);
        }
    }

    async function loadTags() {
        try {
            const result = await getTags();
            tags = Array.isArray(result) ? result : (result?.$values || []);
            console.log(tags);
        } catch (error) {
            tagsError = error;
            console.error("Error fetching tags:", error);
        }
    }

    async function loadCurrentUser() {
        try {
            currentUser = await getCurrentUser();
            console.log('Current user:', currentUser);
        } catch (error) {
            console.error('Error loading current user:', error);
            currentUser = null;
        }
    }

    function clearFilters() {
        selectedLanguage = '';
        selectedLanguageCategory = 'all';
        selectedStatus = '';
        sortOrder = 'newest';
        searchQuery = '';
    }

    function navigateToNewPost() {
        goto('/new-post');
    }

    function navigateToLogin() {
        goto('/login');
    }

    function navigateToPost(id) {
        goto(`/post/${id}`);
    }

    function handleLanguageCategoryChange() {
        selectedLanguage = ''; // Reset language when category changes
    }

    // Auto-refresh posts
    onMount(async () => {
        postsLoading = true;
        await Promise.all([loadPosts(), loadTags(), loadCurrentUser()]);
        postsLoading = false;
        
        // Subscribe to post refresh events
        if (browser) {
            const unsubscribe = refreshPosts.subscribe(value => {
                if (value) {
                    loadPosts();
                }
            });
            return unsubscribe;
        }
    });

    // Fonction pour obtenir l'icône du langage
    function getPostLanguageIcon(language) {
        return getLanguageIcon(language);
    }

    // Fonction pour obtenir le label du langage
    function getPostLanguageLabel(language) {
        return getLanguageLabel(language);
    }

</script>

<section class="questions-section">
    <div class="questions-header">
        <input
            class="search-bar"
            type="text"
            placeholder="  Search questions..."
            bind:value={searchQuery}
        />
        
        <div class="filters-row">
            <div class="filters">
                <select bind:value={selectedLanguageCategory} class="filter-select" on:change={handleLanguageCategoryChange}>
                    {#each languageCategories as category}
                        <option value={category.value}>{category.label}</option>
                    {/each}
                </select>

                <select bind:value={selectedLanguage} class="filter-select">
                    <option value="">All Languages</option>
                    {#each filteredLanguagesForDropdown as language}
                        <option value={language.value}>{language.icon} {language.label}</option>
                    {/each}
                </select>

                <select bind:value={selectedStatus} class="filter-select">
                    <option value="">All Status</option>
                    {#each availableStatuses as status}
                        <option value={status}>{status}</option>
                    {/each}
                </select>

                <select bind:value={sortOrder} class="filter-select">
                    <option value="upvotes">By Upvotes</option>
                    <option value="downvotes">By Downvotes</option>
                    <option value="newest">Newest First</option>
                    <option value="oldest">Oldest First</option>
                </select>

                <button class="clear-filters-btn" on:click={clearFilters}>
                    Clear Filters
                </button>
            </div>
        </div>

        <!-- <div class="tags">
            {#if tagsError}
                <span class="error">Error loading categories: {tagsError.message}</span>
            {:else if tags.length === 0}
                <span>Loading categories...</span>
            {:else}
                {#each tags as tag (tag.id)}
                    <span class="tag" in:fadeScale={{ duration: 350 }} title={tag.description}>{tag.name}</span>
                {/each}
            {/if}
        </div> -->
        
        <div class="header-bottom">
            <div class="results-info">
                {#if postsLoading}
                    Loading...
                {:else}
                    Showing {posts.length} of {allPosts.length} questions
                {/if}
            </div>
            {#if currentUser}
                <button class="ask-btn" on:click={navigateToNewPost}>Ask Question</button>
            {:else}
                <button class="ask-btn" on:click={navigateToLogin}>Login to Ask Question</button>
            {/if}
        </div>
    </div>
    
    <!-- Desktop Table View -->
    <div class="desktop-view">
        <table class="questions-table">
            <thead>
                <tr>
                    <th>Question</th>
                    <th>Author</th>
                    <th>Language</th>
                    <th>Status</th>
                    <th>Votes</th>
                    <th>Replies</th>
                </tr>
            </thead>
            <tbody>
                {#if postsLoading}
                    <tr><td colspan="6" style="text-align:center; color:#888;">Loading questions...</td></tr>
                {:else if postsError}
                    <tr><td colspan="6" style="text-align:center; color:#c00;">Error loading questions: {postsError.message}</td></tr>
                {:else if posts.length === 0}
                    <tr><td colspan="6" style="text-align:center; color:#888;">No questions found matching your filters.</td></tr>
                {:else}
                    {#each posts as post (post.id)}
                        <Post
                            id={post.id}
                            title={post.title}
                            language={post.language}
                            languageIcon={getPostLanguageIcon(post.language)}
                            status={post.status}
                            votes={post.upvotes+post.downvotes}
                            replies={post.commentsCount || 0}
                            userId={post.userId}
                            author={post.author}
                            currentUser={currentUser}
                        />
                    {/each}
                {/if}
            </tbody>
        </table>
    </div>

    <!-- Mobile Card View -->
    <div class="mobile-view">
        {#if postsLoading}
            <div class="loading-message">Loading questions...</div>
        {:else if postsError}
            <div class="error-message">Error loading questions: {postsError.message}</div>
        {:else if posts.length === 0}
            <div class="empty-message">No questions found matching your filters.</div>
        {:else}
            {#each posts as post (post.id)}
                <div class="post-card" on:click={() => navigateToPost(post.id)}>
                    <div class="post-card-header">
                        <h3 class="post-title">{post.title}</h3>
                        <span class="post-language">
                            <span class="lang-icon">{getPostLanguageIcon(post.language)}</span>
                            {getPostLanguageLabel(post.language)}
                        </span>
                    </div>
                    <div class="post-card-stats">
                        <span class="status {post.status === 'Solved' ? 'solved' : 'needhelp'}">
                            {post.status}
                        </span>
                        <div class="stats-row">
                            <span class="stat">
                                <strong>{post.upvotes}</strong> votes
                            </span>
                            <span class="stat">
                                <strong>{post.commentsCount || 0}</strong> replies
                            </span>
                        </div>
                    </div>
                </div>
            {/each}
        {/if}
    </div>
</section>

<style>
    .questions-section {
        max-width: 1200px;
        margin: 20px auto;
        background: #fff;
        border-radius: 16px;
        box-shadow: 0 2px 16px rgba(0, 0, 0, 0.07);
        padding: 32px;
        font-family: "Inter", Arial, sans-serif;
        width: 100%;
        box-sizing: border-box;
    }

    .questions-header {
        display: flex;
        flex-direction: column;
        gap: 16px;
        margin-bottom: 24px;
    }

    .search-bar {
        width: 100%;
        padding: 12px 16px;
        border: 1px solid #e0e0e0;
        border-radius: 8px;
        font-size: 1rem;
        background: #f7f7f7;
        box-sizing: border-box;
    }

    .filters-row {
        display: flex;
        align-items: center;
        gap: 12px;
        width: 100%;
    }

    .filters {
        display: flex;
        align-items: center;
        gap: 12px;
        flex-wrap: wrap;
        width: 100%;
    }

    .filter-select {
        padding: 10px 14px;
        border: 2px solid #e1e5e9;
        border-radius: 8px;
        background: linear-gradient(135deg, #ffffff 0%, #f8f9fa 100%);
        font-size: 0.9rem;
        font-weight: 500;
        color: #24292f;
        cursor: pointer;
        transition: all 0.3s ease;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
        appearance: none;
        background-image: url("data:image/svg+xml;charset=utf-8,%3Csvg xmlns='http://www.w3.org/2000/svg' fill='%23656d76' viewBox='0 0 16 16'%3E%3Cpath d='M8 10.5L3 5.5h10L8 10.5z'/%3E%3C/svg%3E");
        background-repeat: no-repeat;
        background-position: right 12px center;
        background-size: 16px;
        padding-right: 40px;
        min-width: 140px;
        flex: 1;
    }

    .filter-select:hover {
        border-color: #0969da;
        background: linear-gradient(135deg, #f6f8fa 0%, #ffffff 100%);
        box-shadow: 0 4px 8px rgba(9, 105, 218, 0.1);
        transform: translateY(-1px);
    }

    .filter-select:focus {
        outline: none;
        border-color: #0969da;
        background: #ffffff;
        box-shadow: 0 0 0 3px rgba(9, 105, 218, 0.15), 0 4px 8px rgba(9, 105, 218, 0.1);
        transform: translateY(-1px);
    }

    .clear-filters-btn {
        padding: 10px 18px;
        background: linear-gradient(135deg, #f6f8fa 0%, #ffffff 100%);
        border: 2px solid #d73a49;
        border-radius: 8px;
        font-size: 0.9rem;
        font-weight: 600;
        color: #d73a49;
        cursor: pointer;
        transition: all 0.3s ease;
        box-shadow: 0 2px 4px rgba(215, 58, 73, 0.1);
        white-space: nowrap;
    }

    .clear-filters-btn:hover {
        background: linear-gradient(135deg, #d73a49 0%, #cb2431 100%);
        color: white;
        border-color: #d73a49;
        box-shadow: 0 4px 12px rgba(215, 58, 73, 0.3);
        transform: translateY(-2px);
    }

   

    .header-bottom {
        display: flex;
        justify-content: space-between;
        align-items: center;
        gap: 16px;
    }

    .results-info {
        font-size: 0.9rem;
        color: #656d76;
        font-weight: 500;
    }

    .ask-btn {
        background: #0969da;
        color: white;
        border: none;
        border-radius: 8px;
        padding: 10px 20px;
        font-size: 1rem;
        font-weight: 600;
        cursor: pointer;
        transition: all 0.2s ease;
        white-space: nowrap;
    }

    .ask-btn:hover {
        background: #0850b3;
        transform: translateY(-1px);
    }

    /* Desktop Table View */
    .desktop-view {
        display: block;
    }

    .mobile-view {
        display: none;
    }

    .questions-table {
        width: 100%;
        border-collapse: collapse;
        margin-top: 12px;
        background: #fff;
    }

    .questions-table th {
        background: #f8f9fa;
        padding: 12px;
        text-align: left;
        font-weight: 600;
        border-bottom: 2px solid #e9ecef;
        color: #495057;
    }

    /* Mobile Card View Styles */
    .post-card {
        background: #fff;
        border: 1px solid #e9ecef;
        border-radius: 12px;
        padding: 16px;
        margin-bottom: 12px;
        cursor: pointer;
        transition: all 0.2s ease;
        box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
    }

    .post-card:hover {
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
        transform: translateY(-2px);
    }

    .post-card-header {
        display: flex;
        justify-content: space-between;
        align-items: flex-start;
        margin-bottom: 12px;
        gap: 8px;
    }

    .post-title {
        font-size: 1.1rem;
        font-weight: 600;
        color: #0969da;
        margin: 0;
        flex: 1;
        line-height: 1.4;
    }

    .post-language {
        display: flex;
        align-items: center;
        gap: 4px;
        font-size: 0.9rem;
        color: #666;
        white-space: nowrap;
    }

    .lang-icon {
        font-size: 1rem;
    }

    .post-card-stats {
        display: flex;
        flex-direction: column;
        gap: 8px;
    }

    .stats-row {
        display: flex;
        gap: 16px;
        flex-wrap: wrap;
    }

    .stat {
        font-size: 0.9rem;
        color: #666;
    }

    .status {
        padding: 4px 12px;
        border-radius: 6px;
        font-size: 0.85rem;
        font-weight: 600;
        align-self: flex-start;
    }

    .status.solved {
        background: #e6f7ec;
        color: #1a7f37;
    }

    .status.needhelp {
        background: #fff3e6;
        color: #b26a00;
    }

    .loading-message,
    .error-message,
    .empty-message {
        text-align: center;
        padding: 40px;
        color: #666;
        font-size: 1.1rem;
    }

    .error-message {
        color: #c00;
    }

    /* Mobile Responsive Design */
    @media (max-width: 768px) {
        .questions-section {
            margin: 10px auto;
            padding: 16px;
            border-radius: 12px;
        }

        .questions-header {
            gap: 12px;
        }

        .search-bar {
            padding: 10px 12px;
            font-size: 0.95rem;
        }

        .filters {
            flex-direction: column;
            gap: 8px;
        }

        .filter-select {
            width: 100%;
            min-width: auto;
            flex: none;
        }

        .clear-filters-btn {
            width: 100%;
        }

        .header-bottom {
            flex-direction: column;
            gap: 12px;
            text-align: center;
        }

        .ask-btn {
            width: 100%;
            padding: 12px;
        }

        

        /* Switch to mobile card view */
        .desktop-view {
            display: none;
        }

        .mobile-view {
            display: block;
        }
    }

    @media (max-width: 480px) {
        .questions-section {
            margin: 5px auto;
            padding: 12px;
        }

        .post-card {
            padding: 12px;
        }

        .post-card-header {
            flex-direction: column;
            align-items: flex-start;
            gap: 6px;
        }

        .post-title {
            font-size: 1rem;
        }

        .stats-row {
            gap: 12px;
        }

        .stat {
            font-size: 0.85rem;
        }
    }
</style>
