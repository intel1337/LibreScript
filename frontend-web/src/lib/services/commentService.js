import { API_BASE_URL } from '$lib/config.js';

// Get comment count for a specific post
export async function getCommentCountForPost(postId) {
    const data = await fetch(`${API_BASE_URL}/api/comment/post/${postId}/count`);
    if (!data.ok) throw new Error('API error: ' + data.status);
    return await data.json();
}

// Get comments for a specific post
export async function getCommentsForPost(postId) {
    const data = await fetch(`${API_BASE_URL}/api/comment/post/${postId}`);
    if (!data.ok) throw new Error('API error: ' + data.status);
    const response = await data.json();
    
    // The backend now returns the comments in the correct hierarchical structure
    return Array.isArray(response) ? response : [];
}

// Post a new comment
export async function postComment(comment) {
    const data = await fetch(`${API_BASE_URL}/api/comment`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(comment)
    });
    if (!data.ok) throw new Error('API error: ' + data.status);
    return await data.json();
}

// Post a reply to a comment
export async function postReply(parentCommentId, reply) {
    const data = await fetch(`${API_BASE_URL}/api/comment/${parentCommentId}/replies`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(reply)
    });
    
    if (!data.ok) {
        const errorText = await data.text();
        throw new Error(errorText || 'API error: ' + data.status);
    }
    return await data.json();
} 