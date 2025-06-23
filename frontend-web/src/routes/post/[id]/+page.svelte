<script>
    import { onMount } from 'svelte';
    import { page } from '$app/stores';
    import { getPostById, votePost } from '$lib/services/postService.js';
    import { getLanguageIcon, getLanguageLabel } from '$lib/data/languages.js';
    import { postComment, postReply, getCommentsForPost } from '$lib/services/commentService.js';
    import { currentUser } from '$lib/stores/authStore.js';
    import "../../../app.css";

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
                    {post.content}
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
                    placeholder="Write your answer here..."
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
                            <p>{comment.content}</p>
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
                                    placeholder="Write your reply..."
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
                                            <p>{reply.content}</p>
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

    .upvote-count {
        color: #0969da;
    }

    .downvote-count {
        color: #d73a49;
    }

    .post-main {
        flex: 1;
    }

    .post-title {
        margin: 0 0 20px 0;
        color: #24292f;
        font-size: 1.75rem;
        line-height: 1.3;
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

    .author-fullname {
        font-style: italic;
        margin-left: 4px;
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

        .post-title {
            font-size: 1.4rem;
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
</style> 