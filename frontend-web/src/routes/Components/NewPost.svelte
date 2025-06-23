<script>
    import { createPost } from "$lib/services/newPostService.js";
    import { goto } from "$app/navigation";
    import { programmingLanguages, languageCategories } from "$lib/data/languages.js";
    import { currentUser, isLoggedIn } from '$lib/stores/authStore.js';
    import { onMount } from 'svelte';

    let title = '';
    let content = '';
    let selectedLanguage = '';
    let selectedStatus = 'Open';
    let selectedCategory = 'all';
    let loading = false;
    let error = '';

    // Available status options
    const statusOptions = [
        { value: 'Open', label: 'Open' },
        { value: 'Pending', label: 'Pending' },
        { value: 'In Progress', label: 'In Progress' },
        { value: 'Solved', label: 'Solved' },
        { value: 'Closed', label: 'Closed' }
    ];

    // Langages filtrés par catégorie
    $: filteredLanguages = selectedCategory === 'all' 
        ? programmingLanguages 
        : programmingLanguages.filter(lang => lang.category === selectedCategory);

    onMount(() => {
        // Check if user is logged in
        if (!$isLoggedIn || !$currentUser) {
            goto('/login');
            return;
        }
    });

    async function handleCreatePost(e) {
        e.preventDefault();

        if (!title.trim() || !content.trim()) {
            error = 'Please fill in both title and content';
            return;
        }

        // Check if user is logged in
        if (!$currentUser) {
            error = 'You must be logged in to create a post';
            return;
        }

        loading = true;
        error = '';

        // Set defaults if user didn't make selections
        setDefaults();

        try {
            const userId = parseInt($currentUser.userId || $currentUser.id || $currentUser.Id);
            const postData = {
                title: title.trim(),
                content: content.trim(),
                userId: userId,
                language: selectedLanguage || 'other',
                status: selectedStatus
            };

            console.log('Creating post with data:', postData);
            console.log('Current user:', $currentUser);
            await createPost(postData);
            goto('/'); // Redirect to home page after successful creation
        } catch (err) {
            console.error('Error creating post:', err);
            error = err.message || 'Failed to create post';
        } finally {
            loading = false;
        }
    }

    function goBack() {
        goto('/');
    }

    function handleCategoryChange() {
        selectedLanguage = ''; // Reset language when category changes
    }

    // Auto-set defaults if user doesn't make selections
    function setDefaults() {
        if (selectedCategory === 'all') {
            selectedCategory = 'other';
        }
        if (!selectedLanguage) {
            selectedLanguage = 'other';
        }
    }
</script>

<section class="new-post-section">
    <div class="new-post-header">
        <button class="back-btn" on:click={goBack}>← Back to Questions</button>
        <h1>Ask a Question</h1>
        <p>Get help from the community by asking a clear, detailed question.</p>
        <p class="user-info">Posting as: <strong>{$currentUser?.username || 'Loading...'}</strong></p>
    </div>

    <form class="new-post-form" on:submit={handleCreatePost}>
        <div class="form-group">
            <label for="title">Question Title *</label>
            <input 
                id="title"
                type="text" 
                placeholder="What's your programming question?"
                bind:value={title}
                required
                maxlength="200"
            />
            <small class="help-text">Be specific and imagine you're asking a question to another person</small>
        </div>

        <div class="form-row">
            <div class="form-group">
                <label for="category">Language Category</label>
                <select id="category" bind:value={selectedCategory} on:change={handleCategoryChange}>
                    {#each languageCategories as category}
                        <option value={category.value}>{category.label}</option>
                    {/each}
                </select>
                <small class="help-text">Will default to "Other" if not selected</small>
            </div>

            <div class="form-group">
                <label for="language">Programming Language</label>
                <select id="language" bind:value={selectedLanguage}>
                    <option value="">Select a language (optional)</option>
                    {#each filteredLanguages as lang}
                        <option value={lang.value}>{lang.icon} {lang.label}</option>
                    {/each}
                </select>
                <small class="help-text">Will default to "Other" if not selected</small>
            </div>
        </div>

        <div class="form-row">
            <div class="form-group">
                <label for="status">Status</label>
                <select id="status" bind:value={selectedStatus}>
                    {#each statusOptions as status}
                        <option value={status.value}>{status.label}</option>
                    {/each}
                </select>
            </div>
            <div></div> <!-- Empty div for grid alignment -->
        </div>

        <div class="form-group">
            <label for="content">Question Details *</label>
            <textarea 
                id="content"
                placeholder="Provide all the details someone would need to help you. Include relevant code, error messages, and what you've tried."
                bind:value={content}
                required
                rows="10"
                maxlength="5000"
            ></textarea>
            <small class="help-text">The more details you provide, the better help you'll receive</small>
        </div>

        {#if error}
            <div class="error-message">{error}</div>
        {/if}

        <div class="form-actions">
            <button type="button" class="cancel-btn" on:click={goBack} disabled={loading}>
                Cancel
            </button>
            <button type="submit" class="submit-btn" disabled={loading}>
                {loading ? 'Publishing...' : 'Publish Question'}
            </button>
        </div>
    </form>
</section>

<style>
    .new-post-section {
        max-width: 900px;
        margin: 20px auto;
        padding: 32px;
        background: #fff;
        border-radius: 16px;
        box-shadow: 0 2px 16px rgba(0, 0, 0, 0.07);
        font-family: "Inter", Arial, sans-serif;
        box-sizing: border-box;
    }

    .new-post-header {
        text-align: center;
        margin-bottom: 32px;
    }

    .back-btn {
        background: #f8f9fa;
        color: #495057;
        border: 1px solid #dee2e6;
        padding: 8px 16px;
        border-radius: 6px;
        font-size: 0.9rem;
        cursor: pointer;
        transition: all 0.2s ease;
        margin-bottom: 20px;
    }

    .back-btn:hover {
        background: #e9ecef;
        transform: translateY(-1px);
    }

    .new-post-header h1 {
        font-size: 2.5rem;
        color: #212529;
        margin: 0 0 12px 0;
        font-weight: 700;
    }

    .new-post-header p {
        font-size: 1.1rem;
        color: #6c757d;
        margin: 0;
    }

    .user-info {
        font-size: 0.95rem;
        color: #6c757d;
        margin-top: 8px;
        font-style: italic;
    }

    .new-post-form {
        display: flex;
        flex-direction: column;
        gap: 24px;
    }

    .form-row {
        display: grid;
        grid-template-columns: 1fr 1fr;
        gap: 20px;
    }

    .form-group {
        display: flex;
        flex-direction: column;
        gap: 8px;
    }

    label {
        font-weight: 600;
        color: #495057;
        font-size: 1rem;
    }

    input[type="text"],
    textarea {
        padding: 12px 16px;
        border: 2px solid #e9ecef;
        border-radius: 8px;
        font-size: 1rem;
        background: #fff;
        transition: border-color 0.2s ease;
        font-family: inherit;
        box-sizing: border-box;
    }

    input[type="text"]:focus,
    textarea:focus {
        outline: none;
        border-color: #0969da;
        box-shadow: 0 0 0 3px rgba(9, 105, 218, 0.1);
    }

    /* Modern dropdown styles matching homepage */
    select {
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
        font-family: inherit;
        box-sizing: border-box;
    }

    select:hover {
        border-color: #0969da;
        background: linear-gradient(135deg, #f6f8fa 0%, #ffffff 100%);
        box-shadow: 0 4px 8px rgba(9, 105, 218, 0.1);
        transform: translateY(-1px);
    }

    select:focus {
        outline: none;
        border-color: #0969da;
        background: #ffffff;
        box-shadow: 0 0 0 3px rgba(9, 105, 218, 0.15), 0 4px 8px rgba(9, 105, 218, 0.1);
        transform: translateY(-1px);
    }

    textarea {
        resize: vertical;
        min-height: 120px;
        line-height: 1.5;
    }

    .help-text {
        font-size: 0.85rem;
        color: #6c757d;
        margin-top: 4px;
    }

    .error-message {
        background: #f8d7da;
        color: #721c24;
        padding: 12px 16px;
        border-radius: 8px;
        border: 1px solid #f5c6cb;
        font-size: 0.9rem;
    }

    .form-actions {
        display: flex;
        gap: 16px;
        justify-content: flex-end;
        margin-top: 16px;
    }

    .cancel-btn,
    .submit-btn {
        padding: 12px 24px;
        border-radius: 8px;
        font-size: 1rem;
        font-weight: 600;
        cursor: pointer;
        transition: all 0.2s ease;
        border: none;
    }

    .cancel-btn {
        background: #f8f9fa;
        color: #495057;
        border: 1px solid #dee2e6;
    }

    .cancel-btn:hover:not(:disabled) {
        background: #e9ecef;
        transform: translateY(-1px);
    }

    .submit-btn {
        background: #0969da;
        color: white;
        min-width: 160px;
    }

    .submit-btn:hover:not(:disabled) {
        background: #0850b3;
        transform: translateY(-1px);
    }

    .submit-btn:disabled,
    .cancel-btn:disabled {
        opacity: 0.6;
        cursor: not-allowed;
        transform: none;
    }

    /* Mobile Responsive Design */
    @media (max-width: 768px) {
        .new-post-section {
            margin: 10px auto;
            padding: 20px;
        }

        .new-post-header h1 {
            font-size: 2rem;
        }

        .new-post-header p {
            font-size: 1rem;
        }

        .form-row {
            grid-template-columns: 1fr;
            gap: 16px;
        }

        .form-actions {
            flex-direction: column;
            gap: 12px;
        }

        .cancel-btn,
        .submit-btn {
            width: 100%;
            padding: 14px;
        }
    }

    @media (max-width: 480px) {
        .new-post-section {
            margin: 5px auto;
            padding: 16px;
        }

        .new-post-header {
            margin-bottom: 24px;
        }

        .new-post-header h1 {
            font-size: 1.75rem;
        }

        .new-post-form {
            gap: 20px;
        }

        input[type="text"],
        select,
        textarea {
            padding: 10px 12px;
            font-size: 0.95rem;
        }

        textarea {
            min-height: 100px;
        }

        .help-text {
            font-size: 0.8rem;
        }
    }
</style>