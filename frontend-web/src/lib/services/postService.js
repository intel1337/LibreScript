import { BaseService } from './BaseService.js';

/**
 * @class PostService
 * @extends BaseService
 * @description Gestion des posts
 */
class PostService extends BaseService {
    constructor() {
        super('PostService');
    }

    /**
     * @returns {Promise<Array>} Tableau de posts
     */
    async getPosts() {
        try {
            const posts = await this.get('/api/post');
            return Array.isArray(posts) ? posts : [];
        } catch (error) {
            this.logError('Failed to get posts', error);
            throw error;
        }
    }

    /**
     * @param {string|number} id - ID du post
     * @returns {Promise<Object>} Données du post
     */
    async getPostById(id) {
        try {
            return await this.get('/api/post/' + id);
        } catch (error) {
            this.logError('Failed to get post ' + id, error);
            throw error;
        }
    }

    /**
     * @param {string|number} postId - ID du post
     * @returns {Promise<Object>} Résultat du vote
     */
    async upvotePost(postId) {
        try {
            return await this.post('/api/post/' + postId + '/upvote');
        } catch (error) {
            this.logError('Failed to upvote post ' + postId, error);
            this.handleAuthError(error);
        }
    }

    /**
     * @param {string|number} postId - ID du post
     * @returns {Promise<Object>} Résultat du vote
     */
    async downvotePost(postId) {
        try {
            return await this.post('/api/post/' + postId + '/downvote');
        } catch (error) {
            this.logError('Failed to downvote post ' + postId, error);
            this.handleAuthError(error);
        }
    }

    /**
     * @param {string|number} id - ID du post
     * @param {string} voteType - 'upvote' ou 'downvote'
     * @returns {Promise<Object>} Résultat du vote
     */
    async votePost(id, voteType) {
        if (voteType === 'upvote') {
            return await this.upvotePost(id);
        } else if (voteType === 'downvote') {
            return await this.downvotePost(id);
        } else {
            throw new Error('Invalid vote type: ' + voteType);
        }
    }

    /**
     * @param {string|number} id - ID du post
     * @returns {Promise<Object>} Résultat de la suppression
     */
    async deletePost(id) {
        try {
            return await this.delete('/api/post/' + id);
        } catch (error) {
            this.logError('Failed to delete post ' + id, error);
            this.handleAuthError(error);
        }
    }

    /**
     * @param {Object} postData - Données du post
     * @returns {Promise<Object>} Post créé
     */
    async createPost(postData) {
        try {
            return await this.post('/api/post', postData);
        } catch (error) {
            this.logError('Failed to create post', error);
            this.handleAuthError(error);
        }
    }

    /**
     * @param {string|number} id - ID du post
     * @param {Object} postData - Données du post
     * @returns {Promise<Object>} Post mis à jour
     */
    async updatePost(id, postData) {
        try {
            return await this.put('/api/post/' + id, postData);
        } catch (error) {
            this.logError('Failed to update post ' + id, error);
            this.handleAuthError(error);
        }
    }
}

// Instance PostService
const postServiceInstance = new PostService();

export async function getPosts() {
    return postServiceInstance.getPosts();
}

export async function getPostById(id) {
    return postServiceInstance.getPostById(id);
}

export async function upvotePost(postId) {
    return postServiceInstance.upvotePost(postId);
}

export async function downvotePost(postId) {
    return postServiceInstance.downvotePost(postId);
}

export async function votePost(id, voteType) {
    return postServiceInstance.votePost(id, voteType);
}

export async function deletePost(id) {
    return postServiceInstance.deletePost(id);
}

export async function createPost(postData) {
    return postServiceInstance.createPost(postData);
}

export async function updatePost(id, postData) {
    return postServiceInstance.updatePost(id, postData);
}

// Instance PostService
export { PostService };