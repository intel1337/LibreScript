export async function getPosts() {
    const data = await fetch('http://localhost:5028/api/post'); 
    if (!data.ok) throw new Error('API error: ' + data.status);
    return await data.json();
}

// Get a single post by ID
export async function getPostById(id) {
    const data = await fetch(`http://localhost:5028/api/post/${id}`);
    if (!data.ok) throw new Error('API error: ' + data.status);
    return await data.json();
}

// Upvote a post
export async function upvotePost(postId) {
    const token = localStorage.getItem('token');
    console.log('Upvote request - Token:', token);
    console.log('Upvote request - Post ID:', postId);
    
    if (!token) {
        throw new Error('No authentication token found');
    }

    const response = await fetch(`http://localhost:5028/api/post/${postId}/upvote`, {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        }
    });
    console.log('Upvote response status:', response.status);
    
    if (!response.ok) {
        const errorText = await response.text();
        console.error('Upvote error response:', errorText);
        throw new Error(`API error: ${response.status}`);
    }

    return await response.json();
}

// Downvote a post
export async function downvotePost(postId) {
    const token = localStorage.getItem('token');
    console.log('Downvote request - Token:', token);
    console.log('Downvote request - Post ID:', postId);
    
    if (!token) {
        throw new Error('No authentication token found');
    }

    const response = await fetch(`http://localhost:5028/api/post/${postId}/downvote`, {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        }
    });
    console.log('Downvote response status:', response.status);
    
    if (!response.ok) {
        const errorText = await response.text();
        console.error('Downvote error response:', errorText);
        throw new Error(`API error: ${response.status}`);
    }

    return await response.json();
}

// Vote on a post (upvote or downvote)
export async function votePost(id, voteType) {
    if (voteType === 'upvote') {
        return await upvotePost(id);
    } else if (voteType === 'downvote') {
        return await downvotePost(id);
    } else {
        throw new Error('Invalid vote type. Must be "upvote" or "downvote"');
    }
}


export async function deletePost(id) {
    const token = localStorage.getItem('token');
    const data = await fetch(`http://localhost:5028/api/post/${id}`, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        }
    });
    if (!data.ok) throw new Error('API error: ' + data.status);
    return await data.json();
}