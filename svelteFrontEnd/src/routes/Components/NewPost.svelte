<script>
    import { createPost } from "$lib/services/newPostService.js";
    import { onMount } from "svelte";   
    import { getTags } from "$lib/services/tagService.js";
    import { goto } from "$app/navigation";
    import { programmingLanguages, languageCategories } from "$lib/data/languages.js";

    let title = '';
    let content = '';
    let selectedLanguage = '';
    let selectedTag = '';
    let selectedCategory = 'all';
    let tags = [];
    let loading = false;
    let error = '';

    // Langages filtrés par catégorie
    $: filteredLanguages = selectedCategory === 'all' 
        ? programmingLanguages 
        : programmingLanguages.filter(lang => lang.category === selectedCategory);

    onMount(async () => {
        // Load tags
        try {
            const result = await getTags();
            tags = result?.$values || [];
            console.log('Tags loaded:', tags.length);
        } catch (err) {
            console.error('Error loading tags:', err);
            // Don't fail the whole component for tags
        }
    });

    async function handleCreatePost(e) {
        e.preventDefault();

        if (!title.trim() || !content.trim()) {
            error = 'Please fill in both title and content';
            return;
        }

        loading = true;
        error = '';

        try {
            const postData = {
                title: title.trim(),
                content: content.trim(),
                userId: 1, // Default user ID for anonymous posts
                language: selectedLanguage || '',
                status: 'Open'
            };

            console.log('Creating post with data:', postData);
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
</script>

<section class="new-post-section">
    <div class="new-post-header">
        <button class="back-btn" on:click={goBack}>← Back to Questions</button>
        <h1>Ask a Question</h1>
        <p>Get help from the community by asking a clear, detailed question.</p>
        <p class="user-info">Posting as: <strong>Anonymous</strong></p>
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
            </div>

            <div class="form-group">
                <label for="language">Programming Language</label>
                <select id="language" bind:value={selectedLanguage}>
                    <option value="">Select a language</option>
                    {#each filteredLanguages as lang}
                        <option value={lang.value}>{lang.icon} {lang.label}</option>
                    {/each}
                </select>
            </div>
        </div>

        <div class="form-row">
            <div class="form-group">
                <label for="tag">Category</label>
                <select id="tag" bind:value={selectedTag}>
                    <option value="">Select a category</option>
                    {#each tags as tag}
                        <option value={tag.id}>{tag.name}</option>
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
    select,
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
    select:focus,
    textarea:focus {
        outline: none;
        border-color: #0969da;
        box-shadow: 0 0 0 3px rgba(9, 105, 218, 0.1);
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