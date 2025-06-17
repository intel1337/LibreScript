<script>
// Ce composant doit être utilisé à l'intérieur d'un <tbody> d'une table globale

import { goto } from '$app/navigation';
import { deletePost } from '$lib/services/postService';
import { refreshPosts } from '$lib/stores/postStore.js';

export let id;
export let title;
export let language;
export let languageIcon;
export let status;
export let votes;
export let userId;
export let currentUser = null;

// Check if current user owns this post
$: isOwner = currentUser && currentUser.userId && parseInt(currentUser.userId) === userId;

function navigateToPost(event) {
    // Don't navigate if clicking on delete button
    if (event.target.classList.contains('delete-btn')) {
        return;
    }
    goto(`/post/${id}`);
}

async function handleDeletePost(event) {
    event.stopPropagation(); 
    
    if (!confirm('Are you sure you want to delete this post?')) {
        return;
    }
    
    try {
        await deletePost(id);
        console.log('Post deleted successfully');
        // Trigger refresh of posts list
        refreshPosts.set(true);
    } catch (error) {
        console.error('Error deleting post:', error);
        alert('Error deleting post. Please try again.');
    }
}
</script>

<tr class="post-row" on:click={navigateToPost}>
    <td class="title-cell">{title}</td>
    <td><span class="lang-icon">{languageIcon}</span> {language}</td>
    <td><span class="status {status === 'Solved' ? 'solved' : 'needhelp'}">{status}</span></td>
    <td>{votes}</td>
    <td>
        {#if isOwner}
            <button class="delete-btn" on:click={handleDeletePost}>Supprimer</button>
        {/if}
    </td>
</tr>

<style>
.post-row {
    border-bottom: 1px solid #f0f0f0;
    cursor: pointer;
    transition: background-color 0.2s ease;
}

.post-row:hover {
    background-color: #f8f9fa;
}

td {
    font-size: 1.01rem;
    color: #222;
    padding: 12px 10px;
    text-align: left;
}

.title-cell {
    font-weight: 500;
    color: #0969da;
}

.title-cell:hover {
    text-decoration: underline;
}

.lang-icon {
    margin-right: 6px;
}
.status.solved {
    background: #e6f7ec;
    color: #1a7f37;
    border-radius: 8px;
    padding: 4px 12px;
    font-weight: 600;
    font-size: 0.95rem;
}
.status.needhelp {
    background: #fff3e6;
    color: #b26a00;
    border-radius: 8px;
    padding: 4px 12px;
    font-weight: 600;
    font-size: 0.95rem;
}

.delete-btn {
    background: #dc3545;
    color: white;
    border: none;
    border-radius: 6px;
    padding: 6px 12px;
    font-size: 0.85rem;
    font-weight: 600;
    cursor: pointer;
    transition: all 0.2s ease;
}

.delete-btn:hover {
    background: #c82333;
    transform: translateY(-1px);
}
</style>