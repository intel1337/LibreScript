// src/lib/services/registerService.js
import { BaseService } from './BaseService.js';

/**
 * @class RegisterService
 * @extends BaseService
 * @description Gestion de l'inscription des utilisateurs
 */
class RegisterService extends BaseService {
    constructor() {
        super('RegisterService');
    }

    /**
     * @param {Object} userData - Données de l'utilisateur
     * @returns {Promise<Object>} Résultat de l'inscription
     */
    async register({ username, fullName, password, email }) {
        try {
            const response = await this.post('/api/user/register', 
                { username, fullName, password, email },
                { headers: this.getHeaders(false) }
            );

            if (response && response.token) {
                return { success: true, token: response.token };
            }
            return response;
        } catch (error) {
            this.logError('Registration failed', error);
            
            let errorMessage = 'Erreur lors de l\'inscription.';
            if (error.message.includes('409')) {
                errorMessage = 'Nom d\'utilisateur ou email déjà existant.';
            } else if (error.message.includes('400')) {
                errorMessage = 'Données d\'inscription invalides.';
            } else if (error.message.includes('500')) {
                errorMessage = 'Erreur serveur, veuillez réessayer plus tard.';
            }
            
            return { error: errorMessage };
        }
    }

    /**
     * @param {string} username - Nom d'utilisateur à vérifier
     * @returns {Promise<Object>} Résultat de la vérification
     */
    async checkUsernameAvailability(username) {
        try {
            return await this.get('/api/user/check-username/' + username);
        } catch (error) {
            this.logError('Failed to check username availability', error);
            throw error;
        }
    }

    /**
     * @param {string} email - Email à vérifier
     * @returns {Promise<Object>} Résultat de la vérification
     */
    async checkEmailAvailability(email) {
        try {
            return await this.get('/api/user/check-email/' + email);
        } catch (error) {
            this.logError('Failed to check email availability', error);
            throw error;
        }
    }
}

// Instance RegisterService
const registerServiceInstance = new RegisterService();

export async function register(userData) {
    return registerServiceInstance.register(userData);
}

export async function checkUsernameAvailability(username) {
    return registerServiceInstance.checkUsernameAvailability(username);
}

export async function checkEmailAvailability(email) {
    return registerServiceInstance.checkEmailAvailability(email);
}

// Instance RegisterService
export { RegisterService };
