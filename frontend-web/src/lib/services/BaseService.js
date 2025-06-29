import { API_BASE_URL } from '$lib/config.js';
import { goto } from '$app/navigation';

/**
 * @class BaseService
 * @description Classe de base pour tous les services
 */
export class BaseService {
    constructor(serviceName = 'BaseService') {
        this.serviceName = serviceName;
        this.baseUrl = API_BASE_URL;
    }

    /**
     * @returns {string|null} Token d'authentification
     */
    getToken() {
        if (typeof localStorage === 'undefined') return null;
        return localStorage.getItem('token');
    }

    /**
     * @param {string} token 
     */
    storeToken(token) {
        if (typeof localStorage !== 'undefined' && token) {
            localStorage.setItem('token', token);
        }
    }

    /**
     * Supprime le token d'authentification
     */
    removeToken() {
        if (typeof localStorage !== 'undefined') {
            localStorage.removeItem('token');
        }
    }

    /**
     * @param {boolean} includeAuth - Inclure l'authentification
     * @returns {Object} Headers de requête
     */
    getHeaders(includeAuth = true) {
        const headers = {
            'Content-Type': 'application/json'
        };

        if (includeAuth) {
            const token = this.getToken();
            if (token) {
                headers.Authorization = 'Bearer ' + token;
            }
        }

        return headers;
    }

    /**
     * @param {string} message - Message d'erreur
     * @param {any} data - Données supplémentaires
     */
    logError(message, data) {
        console.error('[' + this.serviceName + '] ' + message, data);
    }

    /**
     * @param {string} url - URL de la requête
     * @param {Object} options - Options de la requête
     * @returns {Promise<any>} Réponse de la requête
     */
    async makeRequest(url, options = {}) {
        try {
            const fullUrl = url.startsWith('http') ? url : this.baseUrl + url;
            
            const config = {
                headers: this.getHeaders(),
                method: options.method || 'GET'
            };

            if (options.body) {
                config.body = options.body;
            }

            if (options.headers) {
                Object.assign(config.headers, options.headers);
            }

            const response = await fetch(fullUrl, config);

            if (!response.ok) {
                const errorText = await response.text();
                throw new Error('API error: ' + response.status + ' - ' + errorText);
            }

            const contentType = response.headers.get('content-type');
            if (contentType && contentType.includes('application/json')) {
                return await response.json();
            }
            
            return await response.text();
        } catch (error) {
            this.logError('Request failed', error);
            throw error;
        }
    }

    /**
     * @param {string} endpoint 
     * @param {Object} options 
     * @returns {Promise<any>}
     */
    async get(endpoint, options = {}) {
        return this.makeRequest(endpoint, { method: 'GET', ...options });
    }

    /**
     * @param {string} endpoint 
     * @param {any} data 
     * @param {Object} options 
     * @returns {Promise<any>}
     */
    async post(endpoint, data = null, options = {}) {
        const config = { method: 'POST', ...options };
        if (data) {
            config.body = JSON.stringify(data);
        }
        return this.makeRequest(endpoint, config);
    }

    /**
     * @param {string} endpoint 
     * @param {any} data 
     * @param {Object} options 
     * @returns {Promise<any>}
     */
    async put(endpoint, data = null, options = {}) {
        const config = { method: 'PUT', ...options };
        if (data) {
            config.body = JSON.stringify(data);
        }
        return this.makeRequest(endpoint, config);
    }

    /**
     * @param {string} endpoint 
     * @param {Object} options 
     * @returns {Promise<any>}
     */
    async delete(endpoint, options = {}) {
        return this.makeRequest(endpoint, { method: 'DELETE', ...options });
    }

    /**
     * @param {Error} error 
     */
    handleAuthError(error) {
        if (error.message.includes('401') || error.message.includes('403')) {
            this.removeToken();
            if (typeof window !== 'undefined') {
                goto('/login');
            }
        }
        throw error;
    }
} 