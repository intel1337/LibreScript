const API_URL = 'http://localhost:5028';

// Get user's posts
export async function getUserPosts() {
    const token = localStorage.getItem('token');
    if (!token) {
        throw new Error('No authentication token found');
    }

    console.log('Fetching user posts with token:', token);

    const response = await fetch(`${API_URL}/api/user/posts`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        }
    });

    console.log('User posts response status:', response.status);

    if (!response.ok) {
        const errorText = await response.text();
        console.error('Error fetching user posts:', errorText);
        throw new Error(`API error: ${response.status} - ${errorText}`);
    }

    const posts = await response.json();
    console.log('User posts received:', posts);
    return posts;
}

// Update user profile
export async function updateProfile(profileData) {
    const token = localStorage.getItem('token');
    if (!token) {
        throw new Error('No authentication token found');
    }

    const response = await fetch(`${API_URL}/api/user/profile`, {
        method: 'PUT',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(profileData)
    });

    if (!response.ok) {
        const errorText = await response.text();
        throw new Error(errorText || `API error: ${response.status}`);
    }

    return await response.json();
}

// Update post status
export async function updatePostStatus(postId, status) {
    const token = localStorage.getItem('token');
    if (!token) {
        throw new Error('No authentication token found');
    }

    const response = await fetch(`${API_URL}/api/post/${postId}/status`, {
        method: 'PUT',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ status })
    });

    if (!response.ok) {
        const errorText = await response.text();
        throw new Error(errorText || `API error: ${response.status}`);
    }

    return await response.json();
}

// Delete post
export async function deletePost(postId) {
    const token = localStorage.getItem('token');
    if (!token) {
        throw new Error('No authentication token found');
    }

    const response = await fetch(`${API_URL}/api/post/${postId}`, {
        method: 'DELETE',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        }
    });

    if (!response.ok) {
        const errorText = await response.text();
        throw new Error(errorText || `API error: ${response.status}`);
    }

    return response.status === 204;
} 