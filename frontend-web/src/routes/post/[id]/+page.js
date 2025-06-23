import { getPostById } from '$lib/services/postService.js';
import { getCommentsForPost } from '$lib/services/commentService.js';

export async function load({ params }) {
    const postId = parseInt(params.id);
    
    try {
        const post = await getPostById(postId);
        const comments = await getCommentsForPost(postId);
        
        return {
            post,
            comments
        };
    } catch (error) {
        console.error('Error loading post:', error);
        return {
            post: null,
            comments: [],
            error: error.message
        };
    }
} 