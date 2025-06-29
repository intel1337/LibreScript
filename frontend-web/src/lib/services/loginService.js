import { BaseService } from './BaseService.js';
import { authStore } from '$lib/stores/authStore.js';

/**
 * @class LoginService
 * @extends BaseService
 * @description Gestion de la connexion utilisateur
 */
class LoginService extends BaseService {
    constructor() {
        super('LoginService');
    }

    /**
     * @returns {boolean} True si l'utilisateur est connecté
     */
    isLoggedIn() {
        return !!this.getToken();
    }

    /**
     * @param {string} username - Nom d'utilisateur
     * @param {string} password - Mot de passe
     * @returns {Promise<Object>} Résultat de la connexion
     */
    async login(username, password) {
        try {
            const response = await this.post('/api/user/login', 
                { username, password },
                { headers: this.getHeaders(false) }
            );

            if (response && response.token) {
                this.storeToken(response.token);
                authStore.setUser(response.user || { username });
                return { success: true, user: response.user, token: response.token };
            }

            return { error: 'Identifiants invalides' };
        } catch (error) {
            this.logError('Login failed', error);
            
            let errorMessage = 'Erreur de connexion';
            if (error.message.includes('401') || error.message.includes('403')) {
                errorMessage = 'Identifiants incorrects';
            } else if (error.message.includes('500')) {
                errorMessage = 'Erreur serveur';
            }
            
            return { error: errorMessage };
        }
    }

    /**
     * @param {Object} userData - Données utilisateur
     * @returns {Promise<Object>} Résultat de l'inscription
     */
    async register(userData) {
        try {
            const response = await this.post('/api/user/register',
                userData,
                { headers: this.getHeaders(false) }
            );

            if (response && response.token) {
                this.storeToken(response.token);
                authStore.setUser(response.user || { username: userData.username });
                return { success: true, user: response.user, token: response.token };
            }

            return response;
        } catch (error) {
            this.logError('Registration failed', error);
            
            let errorMessage = 'Erreur d\'inscription';
            if (error.message.includes('409')) {
                errorMessage = 'Utilisateur déjà existant';
            } else if (error.message.includes('400')) {
                errorMessage = 'Données invalides';
            }
            
            return { error: errorMessage };
        }
    }

    /**
     * Déconnexion utilisateur
     */
    logout() {
        this.removeToken();
        authStore.clearUser();
    }

    /**
     * @returns {Promise<Object|null>} Utilisateur actuel
     */
    async getCurrentUser() {
        const token = this.getToken();
        if (!token) {
            return null;
        }

        try {
            const user = await this.get('/api/user/profile');
            authStore.setUser(user);
            return user;
        } catch (error) {
            this.logError('Failed to get current user', error);
            this.removeToken();
            authStore.clearUser();
            return null;
        }
    }

    /**
     * @returns {Promise<boolean>} True si le token est valide
     */
    async validateTokenWithServer() {
        const token = this.getToken();
        if (!token) {
            return false;
        }

        try {
            await this.get('/api/user/profile');
            return true;
        } catch {
            this.removeToken();
            authStore.clearUser();
            return false;
        }
    }

    /**
     * @returns {boolean} True si l'authentification est requise
     */
    requireAuth() {
        if (!this.isLoggedIn()) {
            if (typeof window !== 'undefined') {
                window.location.href = '/login';
            }
            return false;
        }
        return true;
    }
}

// Instance LoginService
const loginServiceInstance = new LoginService();

export function isLoggedIn() {
    return loginServiceInstance.isLoggedIn();
}

export async function login(username, password) {
    return loginServiceInstance.login(username, password);
}

export async function register(userData) {
    return loginServiceInstance.register(userData);
}

export function logout() {
    return loginServiceInstance.logout();
}

export async function getCurrentUser() {
    return loginServiceInstance.getCurrentUser();
}

export async function validateTokenWithServer() {
    return loginServiceInstance.validateTokenWithServer();
}

export function requireAuth() {
    return loginServiceInstance.requireAuth();
}

// Instance LoginService
export { LoginService };
