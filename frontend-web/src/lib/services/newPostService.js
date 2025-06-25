const API_URL = 'http://192.168.10.106:5028';

export async function createPost(post) {
    try {
        const response = await fetch(`${API_URL}/api/post`, {
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