<script>
    import { onMount } from 'svelte';
    import { page } from '$app/stores';
    import { getPostById, votePost } from '$lib/services/postService.js';
    import { getLanguageIcon, getLanguageLabel } from '$lib/data/languages.js';
    import { postComment, postReply, getCommentsForPost } from '$lib/services/commentService.js';
    import { currentUser } from '$lib/stores/authStore.js';
    import "../../../app.css";
    import { aiService } from '$lib/services/aiService.js';

    let post = null;
    let error = null;
    let loading = false;
    let comments = [];
    let newCommentContent = '';
    let replyContent = {};
    let showReplyForm = {};
    let userVote = null; // null = pas de vote, true = upvote, false = downvote

    onMount(async () => {
        const postId = $page.params.id;
        
        try {
            const result = await getPostById(postId);
            post = result;
            
            // Charger les commentaires séparément
            const loadedComments = await getCommentsForPost(postId);
            // Ensure all comments have a proper replies array structure
            comments = loadedComments.map(comment => ({
                ...comment,
                replies: Array.isArray(comment.replies) ? comment.replies : []
            }));
            
            // Vérifier si l'utilisateur a déjà voté
            userVote = result.userVote ?? null;
        } catch (err) {
            console.error('Error loading post:', err);
            error = err.message || 'Failed to load post';
        }
    });

    async function handleVote(postId, voteType) {
        if (loading) return;
        loading = true;
        
        try {
            const result = await votePost(postId, voteType);
            // Mettre à jour les compteurs de votes
            post.upvotes = result.upvotes;
            post.downvotes = result.downvotes;
            
            // Mettre à jour l'état du vote de l'utilisateur
            if (voteType === 'upvote') {
                userVote = userVote === true ? null : true;
            } else {
                userVote = userVote === false ? null : false;
            }
        } catch (err) {
            console.error('Error voting:', err);
            // Could show error message to user
        } finally {
            loading = false;
        }
    }

    async function handleCommentSubmit() {
        if (!newCommentContent.trim()) return;
        
        // Check if user is logged in
        if (!$currentUser) {
            console.error('User not logged in');
            return;
        }
        

        
        try {
            const userId = parseInt($currentUser.userId || $currentUser.id || $currentUser.Id);
            const comment = {
                content: newCommentContent,
                postId: post.id,
                userId: userId
            };
            
            const newComment = await postComment(comment);
            // Add the new comment with empty replies array
            comments = [...comments, { ...newComment, replies: [] }];
            newCommentContent = '';
        } catch (err) {
            console.error('Error posting comment:', err);
        }
    }

    async function handleReplySubmit(parentCommentId) {
        const content = replyContent[parentCommentId];
        if (!content?.trim()) return;
        
        // Check if user is logged in
        if (!$currentUser) {
            console.error('User not logged in');
            return;
        }
        
        try {
            const userId = parseInt($currentUser.userId || $currentUser.id || $currentUser.Id);
            const reply = {
                content: content,
                userId: userId
            };
            

            
            const newReply = await postReply(parentCommentId, reply);
            
            // Find the parent comment and add the reply
            comments = comments.map(comment => {
                if (comment.id === parentCommentId) {
                    return {
                        ...comment,
                        replies: [...(comment.replies || []), newReply]
                    };
                }
                return comment;
            });
            
            replyContent[parentCommentId] = '';
            showReplyForm[parentCommentId] = false;
        } catch (err) {
            console.error('Error posting reply:', err);
        }
    }

    function toggleReplyForm(commentId) {
        showReplyForm[commentId] = !showReplyForm[commentId];
        showReplyForm = { ...showReplyForm };
    }

    function formatDate(dateString) {
        return new Date(dateString).toLocaleDateString('en-US', {
            year: 'numeric',
            month: 'long',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });
    }
</script>

<svelte:head>
    {#if post}
        <title>{post.title} - LibreScript</title>
        <meta name="description" content={post.content.substring(0, 160)} />
    {:else}
        <title>Loading Post - LibreScript</title>
    {/if}
</svelte:head>

<div class="post-container">
    {#if error}
        <div class="error-message">
            <h2>Error loading post</h2>
            <p>{error}</p>
            <a href="/" class="back-link">← Back to questions</a>
        </div>
    {:else if !post}
        <div class="loading">Loading post...</div>
    {:else}
        <div class="post-header">
            <a href="/" class="back-link">← Back to questions</a>
            <div class="post-meta">
                <span class="language">
                    <span class="lang-icon">{getLanguageIcon(post.language)}</span>
                    {getLanguageLabel(post.language)}
                </span>
                <span class="status {post.status === 'Solved' ? 'solved' : 'needhelp'}">
                    {post.status}
                </span>
                <span class="views">{post.views} views</span>
                <span class="date">Posted {formatDate(post.createdAt)}</span>
            </div>
        </div>

        <div class="post-content">
            <div class="voting-section">
                <div class="vote-item">
                    <button 
                        class="vote-btn upvote" 
                        class:loading
                        class:active={userVote === true}
                        on:click={() => handleVote(post.id, 'upvote')}
                        disabled={loading}
                    >
                        ▲
                    </button>
                    <span class="vote-count">{post.upvotes}</span>
                    <span class="vote-label">Upvotes</span>
                </div>

                <div class="vote-item">
                    <button 
                        class="vote-btn downvote" 
                        class:loading
                        class:active={userVote === false}
                        on:click={() => handleVote(post.id, 'downvote')}
                        disabled={loading}
                    >
                        ▼
                    </button>
                    <span class="vote-count">{post.downvotes}</span>
                    <span class="vote-label">Downvotes</span>
                </div>
            </div>

            <div class="post-body">
                <h1>{post.title}</h1>
                <div class="post-author">
                    <span class="author-label">Asked by:</span>
                    <span class="author-name">{post.author?.username || 'Unknown'}</span>
                </div>
                <div class="post-text">
                    {@html aiService.renderMarkdown(post.content)}
                </div>
            </div>
        </div>

        <div class="post-footer">
            <div class="post-stats">
                <span class="stat">
                    <strong>{post.upvotes}</strong> Upvotes
                </span>
                <span class="stat">
                    <strong>{post.downvotes }</strong> Downvotes
                </span>
                <span class="stat">
                    <strong>{comments.length || 0}</strong> answers
                </span>
            </div>
        </div>

        <div class="comments-section">
            <h2>Answers ({comments.length})</h2>
            
            <div class="new-comment">
                <h3>Your Answer</h3>
                <textarea 
                    bind:value={newCommentContent}
                    placeholder="Write your answer here... (Markdown supported)"
                    rows="4"
                ></textarea>
                <button 
                    class="submit-btn"
                    on:click={handleCommentSubmit}
                    disabled={!newCommentContent.trim()}
                >
                    {#if $currentUser}
                        Post Your Answer
                    {:else}
                        <a href="/login">Login to post an answer</a>
                    {/if}
                </button>
            </div>

            <div class="comments-list">
                {#each comments as comment (comment.id)}
                    <div class="comment">
                        <div class="comment-content">
                            <div class="comment-text">{@html aiService.renderMarkdown(comment.content)}</div>
                            <div class="comment-meta">
                                <span class="comment-author">{comment.user?.username || 'Anonymous'}</span>
                                <span class="comment-date">{formatDate(comment.createdAt)}</span>
                                <button 
                                    class="reply-btn"
                                    on:click={() => toggleReplyForm(comment.id)}
                                >
                                    Reply
                                </button>
                            </div>
                        </div>

                        {#if showReplyForm[comment.id]}
                            <div class="reply-form">
                                <textarea 
                                    bind:value={replyContent[comment.id]}
                                    placeholder="Write your reply... (Markdown supported)"
                                    rows="3"
                                ></textarea>
                                <div class="reply-actions">
                                    <button 
                                        class="submit-btn small"
                                        on:click={() => handleReplySubmit(comment.id)}
                                        disabled={!replyContent[comment.id]?.trim()}
                                    >
                                        {#if $currentUser}
                                            Post Reply
                                        {:else}
                                            <a href="/login">Login to post a reply</a>
                                        {/if}
                                    </button>
                                    <button 
                                        class="cancel-btn"
                                        on:click={() => toggleReplyForm(comment.id)}
                                    >
                                        Cancel
                                    </button>
                                </div>
                            </div>
                        {/if}

                        {#if comment.replies && comment.replies.length > 0}
                            <div class="replies">
                                {#each comment.replies as reply (reply.id)}
                                    <div class="reply">
                                        <div class="reply-content">
                                            <div class="reply-text">{@html aiService.renderMarkdown(reply.content)}</div>
                                            <div class="comment-meta">
                                                <span class="comment-author">{reply.user?.username || 'Anonymous'}</span>
                                                <span class="comment-date">{formatDate(reply.createdAt)}</span>
                                            </div>
                                        </div>
                                    </div>
                                {/each}
                            </div>
                        {/if}
                    </div>
                {/each}
            </div>
        </div>
    {/if}
</div>

<style>
    .post-container {
        max-width: 1000px;
        margin: 0 auto;
        padding: 20px;
        font-family: "Inter", Arial, sans-serif;
    }

    .error-message {
        text-align: center;
        padding: 40px;
        background: #fff;
        border-radius: 12px;
        box-shadow: 0 2px 16px rgba(0, 0, 0, 0.07);
    }

    .error-message h2 {
        color: #d73a49;
        margin-bottom: 10px;
    }

    .loading {
        text-align: center;
        padding: 40px;
        font-size: 1.2rem;
        color: #666;
    }

    .back-link {
        color: #0969da;
        text-decoration: none;
        font-weight: 500;
        margin-bottom: 20px;
        display: inline-block;
    }

    .back-link:hover {
        text-decoration: underline;
    }

    .post-header {
        margin-bottom: 20px;
    }

    .post-meta {
        display: flex;
        gap: 16px;
        align-items: center;
        margin-top: 10px;
        font-size: 0.9rem;
        color: #666;
    }

    .language {
        display: flex;
        align-items: center;
        gap: 4px;
    }

    .status.solved {
        background: #e6f7ec;
        color: #1a7f37;
        border-radius: 6px;
        padding: 2px 8px;
        font-weight: 600;
        font-size: 0.85rem;
    }

    .status.needhelp {
        background: #fff3e6;
        color: #b26a00;
        border-radius: 6px;
        padding: 2px 8px;
        font-weight: 600;
        font-size: 0.85rem;
    }

    .post-content {
        display: flex;
        gap: 20px;
        background: #fff;
        border-radius: 12px;
        box-shadow: 0 2px 16px rgba(0, 0, 0, 0.07);
        padding: 30px;
        margin-bottom: 30px;
    }

    .voting-section {
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: 20px;
        flex-shrink: 0;
    }

    .vote-item {
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: 6px;
    }

    .vote-btn {
        background: #f6f8fa;
        border: 1px solid #d0d7de;
        border-radius: 6px;
        width: 40px;
        height: 40px;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 18px;
        cursor: pointer;
        transition: all 0.2s;
    }

    .vote-btn:hover:not(:disabled) {
        background: #f3f4f6;
        border-color: #8b949e;
    }

    .vote-btn:disabled {
        opacity: 0.6;
        cursor: not-allowed;
    }

    .upvote:hover:not(:disabled) {
        color: #0969da;
        background: #dbeafe;
        border-color: #0969da;
    }

    .downvote:hover:not(:disabled) {
        color: #d73a49;
        background: #fdf2f2;
        border-color: #d73a49;
    }

    .vote-count {
        font-size: 1rem;
        font-weight: bold;
        min-width: 24px;
        text-align: center;
    }




    .post-body {
        font-size: 1.1rem;
        line-height: 1.6;
        color: #24292f;
        white-space: pre-wrap;
    }

    .post-author {
        margin-bottom: 16px;
        font-size: 0.9rem;
        color: #656d76;
    }

    .author-label {
        font-weight: 500;
    }

    .author-name {
        font-weight: 600;
        color: #0969da;
    }



    .comments-section {
        background: #fff;
        border-radius: 12px;
        box-shadow: 0 2px 16px rgba(0, 0, 0, 0.07);
        padding: 30px;
    }

    .comments-section h2 {
        margin: 0 0 25px 0;
        color: #24292f;
        border-bottom: 1px solid #d0d7de;
        padding-bottom: 10px;
    }

    .new-comment {
        margin-bottom: 30px;
        border-bottom: 1px solid #d0d7de;
        padding-bottom: 25px;
    }

    .new-comment h3 {
        margin: 0 0 15px 0;
        color: #24292f;
        font-size: 1.1rem;
    }

    textarea {
        width: 100%;
        border: 1px solid #d0d7de;
        border-radius: 6px;
        padding: 12px;
        font-family: inherit;
        font-size: 1rem;
        resize: vertical;
        margin-bottom: 10px;
    }

    textarea:focus {
        outline: none;
        border-color: #0969da;
        box-shadow: 0 0 0 3px rgba(9, 105, 218, 0.1);
    }

    .submit-btn {
        background: #0969da;
        color: white;
        border: none;
        border-radius: 6px;
        padding: 8px 16px;
        font-size: 0.9rem;
        font-weight: 500;
        cursor: pointer;
        transition: background-color 0.2s;
    }

    .submit-btn:hover:not(:disabled) {
        background: #0550ae;
    }

    .submit-btn:disabled {
        background: #8b949e;
        cursor: not-allowed;
    }

    .submit-btn.small {
        padding: 6px 12px;
        font-size: 0.85rem;
    }

    .cancel-btn {
        background: transparent;
        color: #656d76;
        border: 1px solid #d0d7de;
        border-radius: 6px;
        padding: 6px 12px;
        font-size: 0.85rem;
        cursor: pointer;
        margin-left: 8px;
    }

    .cancel-btn:hover {
        background: #f6f8fa;
    }

    .comment {
        border: 1px solid #d0d7de;
        border-radius: 8px;
        margin-bottom: 16px;
        padding: 20px;
    }

    .comment-content p {
        margin: 0 0 10px 0;
        line-height: 1.5;
        color: #24292f;
    }

    .comment-meta {
        display: flex;
        align-items: center;
        gap: 12px;
        font-size: 0.85rem;
        color: #656d76;
    }

    .comment-author {
        font-weight: 500;
        color: #24292f;
    }

    .reply-btn {
        background: none;
        border: none;
        color: #0969da;
        cursor: pointer;
        font-size: 0.85rem;
        font-weight: 500;
    }

    .reply-btn:hover {
        text-decoration: underline;
    }

    .reply-form {
        margin-top: 15px;
        padding-top: 15px;
        border-top: 1px solid #d0d7de;
    }

    .reply-actions {
        display: flex;
        align-items: center;
    }

    .replies {
        margin-top: 15px;
        margin-left: 20px;
        border-left: 2px solid #d0d7de;
        padding-left: 15px;
    }

    .reply {
        padding: 15px;
        background: #f6f8fa;
        border-radius: 6px;
        margin-bottom: 10px;
    }

    .reply-content p {
        margin: 0 0 8px 0;
        color: #24292f;
    }

    @media (max-width: 768px) {
        .post-container {
            padding: 10px;
        }

        .post-content {
            flex-direction: column;
            padding: 20px;
        }

        .voting-section {
            flex-direction: row;
            justify-content: center;
            gap: 30px;
        }

        .replies {
            margin-left: 10px;
            padding-left: 10px;
        }
    }

    .vote-btn.active {
        background: #dbeafe;
        border-color: #0969da;
        color: #0969da;
    }

    .vote-btn.downvote.active {
        background: #fdf2f2;
        border-color: #d73a49;
        color: #d73a49;
    }

    /* Markdown styles for comments and replies */
    .comment-text, .reply-text {
        line-height: 1.6;
        color: #24292f;
    }

    .comment-text h1, .comment-text h2, .comment-text h3, .comment-text h4, .comment-text h5, .comment-text h6,
    .reply-text h1, .reply-text h2, .reply-text h3, .reply-text h4, .reply-text h5, .reply-text h6 {
        margin: 16px 0 8px 0;
        font-weight: 600;
        color: #24292f;
    }

    .comment-text h1, .reply-text h1 { font-size: 1.5rem; }
    .comment-text h2, .reply-text h2 { font-size: 1.3rem; }
    .comment-text h3, .reply-text h3 { font-size: 1.1rem; }
    .comment-text h4, .reply-text h4 { font-size: 1rem; }
    .comment-text h5, .reply-text h5 { font-size: 0.9rem; }
    .comment-text h6, .reply-text h6 { font-size: 0.85rem; }

    .comment-text p, .reply-text p {
        margin: 0 0 12px 0;
        line-height: 1.6;
    }

    .comment-text ul, .comment-text ol, .reply-text ul, .reply-text ol {
        margin: 8px 0 12px 0;
        padding-left: 24px;
    }

    .comment-text li, .reply-text li {
        margin: 4px 0;
        line-height: 1.5;
    }

    .comment-text blockquote, .reply-text blockquote {
        margin: 12px 0;
        padding: 8px 16px;
        border-left: 4px solid #d0d7de;
        background: #f6f8fa;
        color: #656d76;
        font-style: italic;
    }

    .comment-text code, .reply-text code {
        background: #f6f8fa;
        padding: 2px 6px;
        border-radius: 4px;
        font-family: 'Monaco', 'Menlo', 'Ubuntu Mono', monospace;
        font-size: 0.9em;
        color: #d73a49;
    }

    .comment-text pre, .reply-text pre {
        background: #f6f8fa;
        border: 1px solid #d0d7de;
        border-radius: 6px;
        padding: 12px;
        margin: 12px 0;
        overflow-x: auto;
        font-family: 'Monaco', 'Menlo', 'Ubuntu Mono', monospace;
        font-size: 0.85em;
        line-height: 1.4;
    }

    .comment-text pre code, .reply-text pre code {
        background: none;
        padding: 0;
        color: #24292f;
        border-radius: 0;
    }

    .comment-text a, .reply-text a {
        color: #0969da;
        text-decoration: none;
    }

    .comment-text a:hover, .reply-text a:hover {
        text-decoration: underline;
    }

    .comment-text strong, .reply-text strong {
        font-weight: 600;
    }

    .comment-text em, .reply-text em {
        font-style: italic;
    }

    .comment-text hr, .reply-text hr {
        border: none;
        border-top: 1px solid #d0d7de;
        margin: 16px 0;
    }

    .comment-text table, .reply-text table {
        border-collapse: collapse;
        margin: 12px 0;
        width: 100%;
    }

    .comment-text th, .comment-text td, .reply-text th, .reply-text td {
        border: 1px solid #d0d7de;
        padding: 8px 12px;
        text-align: left;
    }

    .comment-text th, .reply-text th {
        background: #f6f8fa;
        font-weight: 600;
    }

        .comment-text img, .reply-text img {
        max-width: 100%;
        height: auto;
        border-radius: 6px;
        margin: 8px 0;
    }

    /* Styles globaux pour le contenu markdown dans les commentaires et réponses */
    .comment-text :global(h1),
    .comment-text :global(h2),
    .comment-text :global(h3),
    .comment-text :global(h4),
    .comment-text :global(h5),
    .comment-text :global(h6),
    .reply-text :global(h1),
    .reply-text :global(h2),
    .reply-text :global(h3),
    .reply-text :global(h4),
    .reply-text :global(h5),
    .reply-text :global(h6) {
        margin-top: 1.2em;
        margin-bottom: 0.5em;
        font-weight: 600;
        color: #24292f;
        line-height: 1.3;
    }

    .comment-text :global(h1), .reply-text :global(h1) { font-size: 1.6em; }
    .comment-text :global(h2), .reply-text :global(h2) { font-size: 1.4em; }
    .comment-text :global(h3), .reply-text :global(h3) { font-size: 1.2em; }
    .comment-text :global(h4), .reply-text :global(h4) { font-size: 1.1em; }
    .comment-text :global(h5), .reply-text :global(h5) { font-size: 1em; }
    .comment-text :global(h6), .reply-text :global(h6) { font-size: 0.9em; }

    .comment-text :global(p),
    .reply-text :global(p) {
        margin-bottom: 1em;
        line-height: 1.6;
        color: #24292f;
    }

    .comment-text :global(ul),
    .comment-text :global(ol),
    .reply-text :global(ul),
    .reply-text :global(ol) {
        margin: 1em 0;
        padding-left: 1.5em;
    }

    .comment-text :global(li),
    .reply-text :global(li) {
        margin-bottom: 0.3em;
        line-height: 1.5;
    }

    .comment-text :global(blockquote),
    .reply-text :global(blockquote) {
        margin: 1em 0;
        padding: 0.8em 1em;
        border-left: 4px solid #d0d7de;
        background-color: #f6f8fa;
        color: #656d76;
        font-style: italic;
        border-radius: 0 6px 6px 0;
    }

    .comment-text :global(code),
    .reply-text :global(code) {
        background-color: #f6f8fa;
        padding: 2px 6px;
        border-radius: 4px;
        font-family: 'Monaco', 'Menlo', 'Ubuntu Mono', monospace;
        font-size: 0.9em;
        color: #d73a49;
        border: 1px solid #e1e5e9;
    }

    .comment-text :global(pre),
    .reply-text :global(pre) {
        background-color: #f6f8fa;
        border: 1px solid #d0d7de;
        border-radius: 6px;
        padding: 1em;
        overflow-x: auto;
        margin: 1em 0;
        font-family: 'Monaco', 'Menlo', 'Ubuntu Mono', monospace;
        font-size: 0.85em;
        line-height: 1.4;
    }

    .comment-text :global(pre code),
    .reply-text :global(pre code) {
        background-color: transparent;
        padding: 0;
        border: none;
        color: #24292f;
        font-size: inherit;
    }

    .comment-text :global(a),
    .reply-text :global(a) {
        color: #0969da;
        text-decoration: none;
        font-weight: 500;
    }

    .comment-text :global(a:hover),
    .reply-text :global(a:hover) {
        text-decoration: underline;
        color: #0550ae;
    }

    .comment-text :global(strong),
    .reply-text :global(strong) {
        font-weight: 600;
        color: #24292f;
    }

    .comment-text :global(em),
    .reply-text :global(em) {
        font-style: italic;
    }

    .comment-text :global(hr),
    .reply-text :global(hr) {
        border: none;
        border-top: 1px solid #d0d7de;
        margin: 1.5em 0;
    }

    .comment-text :global(table),
    .reply-text :global(table) {
        border-collapse: collapse;
        width: 100%;
        margin: 1em 0;
        border: 1px solid #d0d7de;
        border-radius: 6px;
        overflow: hidden;
    }

    .comment-text :global(th),
    .comment-text :global(td),
    .reply-text :global(th),
    .reply-text :global(td) {
        border: 1px solid #d0d7de;
        padding: 8px 12px;
        text-align: left;
    }

    .comment-text :global(th),
    .reply-text :global(th) {
        background-color: #f6f8fa;
        font-weight: 600;
        color: #24292f;
    }

    .comment-text :global(img),
    .reply-text :global(img) {
        max-width: 100%;
        height: auto;
        border-radius: 6px;
        margin: 8px 0;
        border: 1px solid #d0d7de;
    }

    /* Styles pour les listes imbriquées */
    .comment-text :global(ul ul),
    .comment-text :global(ol ol),
    .comment-text :global(ul ol),
    .comment-text :global(ol ul),
    .reply-text :global(ul ul),
    .reply-text :global(ol ol),
    .reply-text :global(ul ol),
    .reply-text :global(ol ul) {
        margin: 0.5em 0;
    }

         /* Styles pour les citations de code inline dans des éléments spéciaux */
     .comment-text :global(h1 code),
     .comment-text :global(h2 code),
     .comment-text :global(h3 code),
     .comment-text :global(h4 code),
     .comment-text :global(h5 code),
     .comment-text :global(h6 code),
     .reply-text :global(h1 code),
     .reply-text :global(h2 code),
     .reply-text :global(h3 code),
     .reply-text :global(h4 code),
     .reply-text :global(h5 code),
     .reply-text :global(h6 code) {
         font-size: 0.9em;
         background-color: rgba(175, 184, 193, 0.2);
     }

     /* Styles globaux pour le contenu markdown du post principal */
     .post-text :global(h1),
     .post-text :global(h2),
     .post-text :global(h3),
     .post-text :global(h4),
     .post-text :global(h5),
     .post-text :global(h6) {
         margin-top: 1.5em;
         margin-bottom: 0.8em;
         font-weight: 600;
         color: #24292f;
         line-height: 1.3;
     }

     .post-text :global(h1) { font-size: 1.8em; }
     .post-text :global(h2) { font-size: 1.6em; }
     .post-text :global(h3) { font-size: 1.4em; }
     .post-text :global(h4) { font-size: 1.2em; }
     .post-text :global(h5) { font-size: 1.1em; }
     .post-text :global(h6) { font-size: 1em; }

     .post-text :global(p) {
         margin-bottom: 1.2em;
         line-height: 1.7;
         color: #24292f;
         font-size: 1rem;
     }

     .post-text :global(ul),
     .post-text :global(ol) {
         margin: 1.2em 0;
         padding-left: 1.8em;
     }

     .post-text :global(li) {
         margin-bottom: 0.5em;
         line-height: 1.6;
     }

     .post-text :global(blockquote) {
         margin: 1.5em 0;
         padding: 1em 1.2em;
         border-left: 4px solid #0969da;
         background-color: #f6f8fa;
         color: #656d76;
         font-style: italic;
         border-radius: 0 8px 8px 0;
         box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
     }

     .post-text :global(code) {
         background-color: #f6f8fa;
         padding: 3px 8px;
         border-radius: 4px;
         font-family: 'Monaco', 'Menlo', 'Ubuntu Mono', monospace;
         font-size: 0.9em;
         color: #d73a49;
         border: 1px solid #e1e5e9;
         font-weight: 500;
     }

     .post-text :global(pre) {
         background-color: #f6f8fa;
         border: 1px solid #d0d7de;
         border-radius: 8px;
         padding: 1.2em;
         overflow-x: auto;
         margin: 1.5em 0;
         font-family: 'Monaco', 'Menlo', 'Ubuntu Mono', monospace;
         font-size: 0.9em;
         line-height: 1.5;
         box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
     }

     .post-text :global(pre code) {
         background-color: transparent;
         padding: 0;
         border: none;
         color: #24292f;
         font-size: inherit;
         font-weight: normal;
     }

     .post-text :global(a) {
         color: #0969da;
         text-decoration: none;
         font-weight: 500;
         border-bottom: 1px solid transparent;
         transition: all 0.2s ease;
     }

     .post-text :global(a:hover) {
         text-decoration: underline;
         color: #0550ae;
         border-bottom-color: #0550ae;
     }

     .post-text :global(strong) {
         font-weight: 600;
         color: #24292f;
     }

     .post-text :global(em) {
         font-style: italic;
         color: #24292f;
     }

     .post-text :global(hr) {
         border: none;
         border-top: 2px solid #d0d7de;
         margin: 2em 0;
         border-radius: 1px;
     }

     .post-text :global(table) {
         border-collapse: collapse;
         width: 100%;
         margin: 1.5em 0;
         border: 1px solid #d0d7de;
         border-radius: 8px;
         overflow: hidden;
         box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
     }

     .post-text :global(th),
     .post-text :global(td) {
         border: 1px solid #d0d7de;
         padding: 10px 14px;
         text-align: left;
     }

     .post-text :global(th) {
         background-color: #f6f8fa;
         font-weight: 600;
         color: #24292f;
         font-size: 0.95em;
     }

     .post-text :global(img) {
         max-width: 100%;
         height: auto;
         border-radius: 8px;
         margin: 1em 0;
         border: 1px solid #d0d7de;
         box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
     }

     /* Styles pour les listes imbriquées dans le post */
     .post-text :global(ul ul),
     .post-text :global(ol ol),
     .post-text :global(ul ol),
     .post-text :global(ol ul) {
         margin: 0.8em 0;
     }

     /* Styles pour les citations de code inline dans les titres du post */
     .post-text :global(h1 code),
     .post-text :global(h2 code),
     .post-text :global(h3 code),
     .post-text :global(h4 code),
     .post-text :global(h5 code),
     .post-text :global(h6 code) {
         font-size: 0.85em;
         background-color: rgba(175, 184, 193, 0.2);
         padding: 2px 6px;
     }

     /* Style spécial pour les citations dans le post principal */
     .post-text :global(blockquote p) {
         margin-bottom: 0.8em;
     }

     .post-text :global(blockquote p:last-child) {
         margin-bottom: 0;
     }
</style> 