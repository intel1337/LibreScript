import { BaseService } from './BaseService.js';

/**
 * @class NewPostService
 * @extends BaseService
 * @description Gestion de la création et modification des posts
 */
class NewPostService extends BaseService {
    constructor() {
        super('NewPostService');
    }

    /**
     * @param {Object} post - Données du post
     * @returns {Promise<Object>} Post créé
     */
    async createPost(post) {
        try {
            return await this.post('/api/post', post);
        } catch (error) {
            this.logError('Failed to create post', error);
            this.handleAuthError(error);
        }
    }

    /**
     * @param {string|number} id - ID du post
     * @returns {Promise<Object>} Données du post
     */
    async getPost(id) {
        try {
            return await this.get('/api/post/' + id);
        } catch (error) {
            this.logError('Failed to get post ' + id, error);
            throw error;
        }
    }

    /**
     * @param {string|number} id - ID du post
     * @param {Object} post - Données du post
     * @returns {Promise<Object>} Post mis à jour
     */
    async updatePost(id, post) {
        try {
            return await this.put('/api/post/' + id, post);
        } catch (error) {
            this.logError('Failed to update post ' + id, error);
            this.handleAuthError(error);
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
}

const newPostServiceInstance = new NewPostService();

export async function createPost(post) {
    return newPostServiceInstance.createPost(post);
}

export async function getPost(id) {
    return newPostServiceInstance.getPost(id);
}

export async function updatePost(id, post) {
    return newPostServiceInstance.updatePost(id, post);
}

export async function deletePost(id) {
    return newPostServiceInstance.deletePost(id);
}

export { NewPostService };
  