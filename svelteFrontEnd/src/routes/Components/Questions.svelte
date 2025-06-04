<script>

    import { onMount } from "svelte";
    import { getTags } from "$lib/services/tagService.js";
    import { getPosts } from "$lib/services/postService.js";
    import Post from "./Post.svelte";
    import { fade, scale } from 'svelte/transition';
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
    let posts = [];
    let postsError = null;
    let postsLoading = true;

    onMount(async () => {
        try {
            const result = await getTags();
            //   structure { $id, $values: [...] }
            tags = result?.$values || []; // reponse du back ou tableau vide
            console.log(tags);
        } catch (error) {
            tagsError = error;
            console.error("Error fetching tags:", error);
        }

        try {
            const result = await getPosts();
            posts = Array.isArray(result && result.$values) ? result.$values : [];
            console.log(posts);
        } catch (error) {
            postsError = error;
            console.error("Error fetching posts:", error);
        } finally {
            postsLoading = false;
        }
    });
</script>

<section class="questions-section">
    <div class="questions-header">
        <input
            class="search-bar"
            type="text"
            placeholder="  Search questions"
        />
        <div class="tags">
            {#if tagsError}
                <span class="error">Error loading categories: {tagsError.message}</span>
            {:else if tags.length === 0}
                <span>Loading categories...</span>
            {:else}
                {#each tags as tag (tag.id)}
                    <span class="tag" in:fadeScale={{ duration: 350 }} title={tag.description}>{tag.name}</span>
                {/each}
            {/if}
        </div>
        <button class="ask-btn" disabled>Ask Question</button>
    </div>
    <table class="questions-table">
        <thead>
            <tr>
                <th>Question</th>
                <th>Language</th>
                <th>Status</th>
                <th>Votes</th>
                <th>Answers</th>
                <th>Views</th>
            </tr>
        </thead>
        <tbody>
            {#if postsLoading}
                <tr><td colspan="6" style="text-align:center; color:#888;">Loading questions...</td></tr>
            {:else if postsError}
                <tr><td colspan="6" style="text-align:center; color:#c00;">Error loading questions: {postsError.message}</td></tr>
            {:else if posts.length === 0}
                <tr><td colspan="6" style="text-align:center; color:#888;">No questions found.</td></tr>
            {:else}
                {#each posts as post (post.id)}
                    <Post
                        title={post.title}
                        language={post.language}
                        languageIcon={post.languageIcon}
                        status={post.status}
                        votes={post.upvotes}
                        answers={post.answersCount}
                        views={post.views}
                    />
                {/each}
            {/if}
        </tbody>

      
    </table>
</section>

<style>
    .questions-section {
        max-width: 900px;
        margin: 40px auto;
        background: #fff;
        border-radius: 16px;
        box-shadow: 0 2px 16px rgba(0, 0, 0, 0.07);
        padding: 32px 32px 24px 32px;
        font-family: "Inter", Arial, sans-serif;
    }
    .questions-header {
        display: flex;
        flex-direction: column;
        gap: 16px;
        margin-bottom: 24px;
    }
    .search-bar {
        width: 100%;
        padding: 12px 0px 12px 0px;
        border: 1px solid #e0e0e0;
        border-radius: 8px;
        font-size: 1rem;
        background: #f7f7f7;
    }

    .tags {
        display: flex;
        gap: 8px;
        flex-wrap: wrap;
        transition: 0.5s ease;
    }
    .tag {
        background: #f0f0f0;
        color: #333;
        border-radius: 6px;
        padding: 4px 12px;
        font-size: 0.95rem;
        cursor: pointer;
        user-select: none;
    }
    .ask-btn {
        align-self: flex-end;
        background: #f7f7f7;
        color: #333;
        border: 1px solid #e0e0e0;
        border-radius: 8px;
        padding: 8px 20px;
        font-size: 1rem;
        cursor: not-allowed;
        font-weight: 500;
    }
    .questions-table {
        width: 100%;
        border-collapse: collapse;
        margin-top: 12px;
        background: #fff;
    }

    @media (max-width: 700px) {
        .questions-section {
            padding: 12px 2px;
        }
        .questions-header {
            gap: 8px;
        }
        .questions-table th,
        .questions-table td {
            padding: 8px 4px;
            font-size: 0.95rem;
        }
    }
</style>
