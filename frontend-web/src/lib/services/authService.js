import { BaseService } from './BaseService.js';
import { goto } from '$app/navigation';

/**
 * @class AuthService
 * @extends BaseService
 * @description gestion de l'authentification et de la session utilisateur
 */
class AuthService extends BaseService {
    constructor() {
        super('AuthService');
    }

    /**
     * @returns {Promise<Object|null>} Return les infos de l'utilisateur si authentifié, null sinon
     */
    async checkAuth() {
        const token = this.getToken();
        if (!token) {
            if (typeof window !== 'undefined') {
                goto('/login');
            }
            return null;
        }

        try {
            const user = await this.get('/api/user/profile');
            return user;
        } catch (error) {
            this.logError('Auth check failed', error);
            this.handleAuthError(error);
        }
    }

    /**
     * @returns {Promise<Object|null>} Return les infos de l'utilisateur si la session est valide, null sinon
     */
    async validateSession() {
        const token = this.getToken();
        if (!token) {
            return null;
        }

        try {
            const user = await this.get('/api/user/profile');
            return user;
        } catch (error) {
            this.logError('Session validation failed', error);
            this.removeToken();
            return null;
        }
    }

    /**
     * @returns {boolean} True si l'utilisateur est connecté, false sinon
     */
    isLoggedIn() {
        return !!this.getToken();
    }

    /**
     * Force la déconnexion - supprime le token et redirige vers la page de connexion
     */
    logout() {
        this.removeToken();
        if (typeof window !== 'undefined') {
            goto('/login');
        }
    }
}

const authServiceInstance = new AuthService();

export async function checkAuth() {
    return authServiceInstance.checkAuth();
}

export async function validateSession() {
    return authServiceInstance.validateSession();
}

export function isLoggedIn() {
    return authServiceInstance.isLoggedIn();
}

export function logout() {
    return authServiceInstance.logout();
}

// Instance AuthService
export { AuthService };
