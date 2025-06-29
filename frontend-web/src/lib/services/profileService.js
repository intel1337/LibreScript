import { BaseService } from './BaseService.js';

/**
 * @class ProfileService
 * @extends BaseService
 * @description Gestion du profil utilisateur et de ses posts
 */
class ProfileService extends BaseService {
    constructor() {
        super('ProfileService');
    }

    /**
     * @returns {Promise<Array>} Tableau des posts de l'utilisateur
     */
    async getUserPosts() {
        try {
            const posts = await this.get('/api/user/posts');
            return posts;
        } catch (error) {
            this.logError('Failed to get user posts', error);
            this.handleAuthError(error);
        }
    }

    /**
     * @param {Object} profileData - Données du profil à mettre à jour
     * @returns {Promise<Object>} Profil mis à jour
     */
    async updateProfile(profileData) {
        try {
            const result = await this.put('/api/user/profile', profileData);
            return result;
        } catch (error) {
            this.logError('Failed to update profile', error);
            this.handleAuthError(error);
        }
    }

    /**
     * @param {string|number} postId - ID du post
     * @param {string} status - Nouveau statut du post
     * @returns {Promise<Object>} Résultat de la mise à jour du statut
     */
    async updatePostStatus(postId, status) {
        try {
            const result = await this.put('/api/post/' + postId + '/status', { status });
            return result;
        } catch (error) {
            this.logError('Failed to update post ' + postId + ' status', error);
            this.handleAuthError(error);
        }
    }

    /**
     * @param {string|number} postId - ID du post
     * @returns {Promise<boolean>} True si la suppression a réussi
     */
    async deletePost(postId) {
        try {
            await this.delete('/api/post/' + postId);
            return true;
        } catch (error) {
            this.logError('Failed to delete post ' + postId, error);
            this.handleAuthError(error);
        }
    }

    /**
     * @returns {Promise<Object>} Profil de l'utilisateur actuel
     */
    async getCurrentUserProfile() {
        try {
            const profile = await this.get('/api/user/profile');
            return profile;
        } catch (error) {
            this.logError('Failed to get current user profile', error);
            this.handleAuthError(error);
        }
    }
}

// Instance ProfileService
const profileServiceInstance = new ProfileService();

export async function getUserPosts() {
    return profileServiceInstance.getUserPosts();
}

export async function updateProfile(profileData) {
    return profileServiceInstance.updateProfile(profileData);
}

export async function updatePostStatus(postId, status) {
    return profileServiceInstance.updatePostStatus(postId, status);
}

export async function deletePost(postId) {
    return profileServiceInstance.deletePost(postId);
}

export async function getCurrentUserProfile() {
    return profileServiceInstance.getCurrentUserProfile();
}

// Instance ProfileService
export { ProfileService }; 