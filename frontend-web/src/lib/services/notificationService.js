import { BaseService } from './BaseService.js';

/**
 * @class NotificationService
 * @extends BaseService
 * @description Gestion des notifications utilisateur
 */
class NotificationService extends BaseService {
    constructor() {
        super('NotificationService');
    }

    /**
     * @returns {Promise<Array>} Tableau des notifications de l'utilisateur
     */
    async getNotifications() {
        try {
            const notifications = await this.get('/api/notifications');
            return notifications;
        } catch (error) {
            this.logError('Failed to get notifications', error);
            this.handleAuthError(error);
        }
    }

    /**
     * @returns {Promise<number>} Nombre de notifications non lues
     */
    async getUnreadCount() {
        try {
            const result = await this.get('/api/notifications/unread-count');
            return result.count || 0;
        } catch (error) {
            this.logError('Failed to get unread count', error);
            this.handleAuthError(error);
        }
    }

    /**
     * @param {string|number} notificationId - ID de la notification
     * @returns {Promise<Object>} Notification marquée comme lue
     */
    async markAsRead(notificationId) {
        try {
            return await this.put('/api/notifications/' + notificationId + '/read');
        } catch (error) {
            this.logError('Failed to mark notification ' + notificationId + ' as read', error);
            this.handleAuthError(error);
        }
    }

    /**
     * @returns {Promise<Object>} Résultat du marquage
     */
    async markAllAsRead() {
        try {
            return await this.put('/api/notifications/mark-all-read');
        } catch (error) {
            this.logError('Failed to mark all notifications as read', error);
            this.handleAuthError(error);
        }
    }

    /**
     * @param {string|number} notificationId - ID de la notification
     * @returns {Promise<boolean>} True si la suppression a réussi
     */
    async deleteNotification(notificationId) {
        try {
            await this.delete('/api/notifications/' + notificationId);
            return true;
        } catch (error) {
            this.logError('Failed to delete notification ' + notificationId, error);
            this.handleAuthError(error);
        }
    }

    /**
     * @param {Object} notificationData - Données de la notification
     * @returns {Promise<Object>} Notification créée
     */
    async createNotification(notificationData) {
        try {
            return await this.post('/api/notifications', notificationData);
        } catch (error) {
            this.logError('Failed to create notification', error);
            this.handleAuthError(error);
        }
    }

    /**
     * @param {string} type - Type de notification (optionnel)
     * @returns {Promise<Array>} Notifications filtrées par type
     */
    async getNotificationsByType(type) {
        try {
            return await this.get('/api/notifications?type=' + encodeURIComponent(type));
        } catch (error) {
            this.logError('Failed to get notifications by type ' + type, error);
            this.handleAuthError(error);
        }
    }

    /**
     * @returns {Promise<boolean>} True s'il y a de nouvelles notifications
     */
    async hasNewNotifications() {
        try {
            const count = await this.getUnreadCount();
            return count > 0;
        } catch {
            return false;
        }
    }
}

// Instance NotificationService
const notificationServiceInstance = new NotificationService();

export async function getNotifications() {
    return notificationServiceInstance.getNotifications();
}

export async function getUnreadCount() {
    return notificationServiceInstance.getUnreadCount();
}

export async function markAsRead(notificationId) {
    return notificationServiceInstance.markAsRead(notificationId);
}

export async function markAllAsRead() {
    return notificationServiceInstance.markAllAsRead();
}

export async function deleteNotification(notificationId) {
    return notificationServiceInstance.deleteNotification(notificationId);
}

export async function createNotification(notificationData) {
    return notificationServiceInstance.createNotification(notificationData);
}

export async function getNotificationsByType(type) {
    return notificationServiceInstance.getNotificationsByType(type);
}

export async function hasNewNotifications() {
    return notificationServiceInstance.hasNewNotifications();
}

// Instance NotificationService
export { NotificationService }; 