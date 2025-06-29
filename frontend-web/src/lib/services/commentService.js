import { BaseService } from './BaseService.js';

/**
 * @class CommentService
 * @extends BaseService
 * @description Gestion des commentaires
 */
class CommentService extends BaseService {
    constructor() {
        super('CommentService');
    }

    /**
     * @param {string|number} postId - ID du post
     * @returns {Promise<number>} Nombre de commentaires
     */
    async getCommentCountForPost(postId) {
        try {
            const result = await this.get('/api/comment/post/' + postId + '/count');
            return result.count || 0;
        } catch (error) {
            this.logError('Failed to get comment count for post ' + postId, error);
            return 0;
        }
    }

    /**
     * @param {string|number} postId - ID du post
     * @returns {Promise<Array>} Tableau de commentaires en structure hiérarchique
     */
    async getCommentsForPost(postId) {
        try {
            const response = await this.get('/api/comment/post/' + postId);
            const comments = Array.isArray(response) ? response : [];
            return comments;
        } catch (error) {
            this.logError('Failed to get comments for post ' + postId, error);
            return [];
        }
    }

    /**
     * @param {Object} comment - Données du commentaire
     * @returns {Promise<Object>} Commentaire créé
     */
    async postComment(comment) {
        try {
            return await this.post('/api/comment', comment);
        } catch (error) {
            this.logError('Failed to post comment', error);
            this.handleAuthError(error);
        }
    }

    /**
     * @param {string|number} parentCommentId - ID du commentaire parent
     * @param {Object} reply - Données de la réponse
     * @returns {Promise<Object>} Réponse créée
     */
    async postReply(parentCommentId, reply) {
        try {
            return await this.post('/api/comment/' + parentCommentId + '/reply', reply);
        } catch (error) {
            this.logError('Failed to post reply to comment ' + parentCommentId, error);
            this.handleAuthError(error);
        }
    }

    /**
     * @param {string|number} commentId - ID du commentaire
     * @returns {Promise<Object>} Résultat de la suppression
     */
    async deleteComment(commentId) {
        try {
            return await this.delete('/api/comment/' + commentId);
        } catch (error) {
            this.logError('Failed to delete comment ' + commentId, error);
            this.handleAuthError(error);
        }
    }

    /**
     * @param {string|number} commentId - ID du commentaire
     * @param {Object} commentData - Données du commentaire à mettre à jour
     * @returns {Promise<Object>} Commentaire mis à jour
     */
    async updateComment(commentId, commentData) {
        try {
            return await this.put('/api/comment/' + commentId, commentData);
        } catch (error) {
            this.logError('Failed to update comment ' + commentId, error);
            this.handleAuthError(error);
        }
    }
}

// Instance CommentService
const commentServiceInstance = new CommentService();

export async function getCommentCountForPost(postId) {
    return commentServiceInstance.getCommentCountForPost(postId);
}

export async function getCommentsForPost(postId) {
    return commentServiceInstance.getCommentsForPost(postId);
}

export async function postComment(comment) {
    return commentServiceInstance.postComment(comment);
}

export async function postReply(parentCommentId, reply) {
    return commentServiceInstance.postReply(parentCommentId, reply);
}

export async function deleteComment(commentId) {
    return commentServiceInstance.deleteComment(commentId);
}

export async function updateComment(commentId, commentData) {
    return commentServiceInstance.updateComment(commentId, commentData);
}

// Instance CommentService
export { CommentService }; 