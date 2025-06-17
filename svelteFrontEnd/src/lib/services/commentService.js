// Get comments for a specific post
export async function getCommentsForPost(postId) {
    console.log('Fetching comments for post:', postId);
    const data = await fetch(`http://localhost:5028/api/comment`);
    if (!data.ok) throw new Error('API error: ' + data.status);
    const response = await data.json();
    
    const allComments = Array.isArray(response) ? response : (response.$values || []);
    console.log('All comments:', allComments);
    
    // Log the first comment to see its structure
    if (allComments.length > 0) {
        console.log('First comment structure:', allComments[0]);
    }
    
    const postComments = allComments.filter(comment => {
        console.log('Checking comment:', comment, 'postId:', comment.postId, 'expected postId:', postId);
        return comment.postId === postId;
    });
    console.log('Post comments:', postComments);
    
    const parentComments = postComments.filter(comment => {
        console.log('Checking parent comment:', comment, 'parentCommentId:', comment.parentCommentId);
        return !comment.parentCommentId;
    });
    console.log('Parent comments:', parentComments);
    
    // Récupérer les réponses pour chaque commentaire parent
    const commentsWithReplies = await Promise.all(parentComments.map(async parent => {
        console.log('Fetching replies for comment:', parent.id);
        const repliesData = await fetch(`http://localhost:5028/api/comment/${parent.id}/replies`);
        if (!repliesData.ok) throw new Error('API error: ' + repliesData.status);
        const repliesResponse = await repliesData.json();
        const parentReplies = Array.isArray(repliesResponse) ? repliesResponse : (repliesResponse.$values || []);
        console.log('Replies for comment', parent.id, ':', parentReplies);
        
        return {
            ...parent,
            replies: parentReplies
        };
    }));
    
    console.log('Final comments with replies:', commentsWithReplies);
    return commentsWithReplies;
}

// Post a new comment
export async function postComment(comment) {
    const data = await fetch('http://localhost:5028/api/comment', {
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
    const data = await fetch(`http://localhost:5028/api/comment/${parentCommentId}/replies`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(reply)
    });
    if (!data.ok) throw new Error('API error: ' + data.status);
    return await data.json();
} 