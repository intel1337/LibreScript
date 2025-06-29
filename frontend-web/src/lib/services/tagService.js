import { BaseService } from './BaseService.js';

/**
 * @class TagService
 * @extends BaseService
 * @description Gestion des tags/catégories
 */
class TagService extends BaseService {
    constructor() {
        super('TagService');
    }

    /**
     * @returns {Promise<Array>} Tableau des tags/catégories
     */
    async getTags() {
        try {
            const tags = await this.get('/api/category');
            return tags;
        } catch (error) {
            this.logError('Failed to get tags', error);
            throw error;
        }
    }

    /**
     * @param {string|number} tagId - ID du tag
     * @returns {Promise<Object>} Données du tag
     */
    async getTagById(tagId) {
        try {
            return await this.get('/api/category/' + tagId);
        } catch (error) {
            this.logError('Failed to get tag ' + tagId, error);
            throw error;
        }
    }

    /**
     * @param {Object} tagData - Données du tag à créer
     * @returns {Promise<Object>} Tag créé
     */
    async createTag(tagData) {
        try {
            return await this.post('/api/category', tagData);
        } catch (error) {
            this.logError('Failed to create tag', error);
            this.handleAuthError(error);
        }
    }

    /**
     * @param {string|number} tagId - ID du tag
     * @param {Object} tagData - Données du tag à mettre à jour
     * @returns {Promise<Object>} Tag mis à jour
     */
    async updateTag(tagId, tagData) {
        try {
            return await this.put('/api/category/' + tagId, tagData);
        } catch (error) {
            this.logError('Failed to update tag ' + tagId, error);
            this.handleAuthError(error);
        }
    }

    /**
     * @param {string|number} tagId - ID du tag
     * @returns {Promise<boolean>} True si la suppression a réussi
     */
    async deleteTag(tagId) {
        try {
            await this.delete('/api/category/' + tagId);
            return true;
        } catch (error) {
            this.logError('Failed to delete tag ' + tagId, error);
            this.handleAuthError(error);
        }
    }

    /**
     * @param {string} searchTerm - Terme de recherche
     * @returns {Promise<Array>} Tags correspondants
     */
    async searchTags(searchTerm) {
        try {
            return await this.get('/api/category/search?q=' + encodeURIComponent(searchTerm));
        } catch (error) {
            this.logError('Failed to search tags', error);
            throw error;
        }
    }
}

// Instance TagService
const tagServiceInstance = new TagService();

export async function getTags() {
    return tagServiceInstance.getTags();
}

export async function getTagById(tagId) {
    return tagServiceInstance.getTagById(tagId);
}

export async function createTag(tagData) {
    return tagServiceInstance.createTag(tagData);
}

export async function updateTag(tagId, tagData) {
    return tagServiceInstance.updateTag(tagId, tagData);
}

export async function deleteTag(tagId) {
    return tagServiceInstance.deleteTag(tagId);
}

export async function searchTags(searchTerm) {
    return tagServiceInstance.searchTags(searchTerm);
}

// Instance TagService
export { TagService };