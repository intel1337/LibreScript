import { API_BASE_URL } from '$lib/config.js';

export async function createPost(post) {
    try {
        const response = await fetch(`${API_BASE_URL}/api/post`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(post),
        });
        if (!response.ok) {
            throw new Error('Failed to create post');
        }
        return response.json();
    } catch (error) {
        console.error('Error creating post:', error);
        throw error;
    }
}   