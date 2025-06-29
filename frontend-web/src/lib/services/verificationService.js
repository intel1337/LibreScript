import { BaseService } from './BaseService.js';

/**
 * @class VerificationService
 * @extends BaseService
 * @description Gestion de la vérification des utilisateurs
 */
class VerificationService extends BaseService {
    constructor() {
        super('VerificationService');
    }

    /**
     * @returns {Promise<Object>} Statut de vérification de l'utilisateur
     */
    async getVerificationStatus() {
        try {
            const status = await this.get('/api/verification/status');
            return status;
        } catch (error) {
            this.logError('Failed to get verification status', error);
            this.handleAuthError(error);
        }
    }

    /**
     * @returns {Promise<Object>} Résultat de l'envoi du code de vérification
     */
    async sendVerificationCode() {
        try {
            const result = await this.post('/api/verification/send-code');
            return result;
        } catch (error) {
            this.logError('Failed to send verification code', error);
            this.handleAuthError(error);
        }
    }

    /**
     * @param {string} code - Code de vérification
     * @returns {Promise<Object>} Résultat de la vérification du code
     */
    async verifyCode(code) {
        try {
            const result = await this.post('/api/verification/verify', { code });
            return result;
        } catch (error) {
            this.logError('Failed to verify code', error);
            this.handleAuthError(error);
        }
    }

    /**
     * @returns {Promise<boolean>} True si l'utilisateur est vérifié
     */
    async isUserVerified() {
        try {
            const status = await this.getVerificationStatus();
            return status && status.verified === true;
        } catch {
            return false;
        }
    }

    /**
     * @returns {Promise<Object>} Résultat du renvoi
     */
    async resendVerificationCode() {
        try {
            const result = await this.sendVerificationCode();
            return result;
        } catch (error) {
            this.logError('Failed to resend verification code', error);
            throw error;
        }
    }
}

// Instance VerificationService
const verificationServiceInstance = new VerificationService();

// Export objet pour maintenir la compatibilité descendante
export const verificationService = {
    getVerificationStatus: () => verificationServiceInstance.getVerificationStatus(),
    sendVerificationCode: () => verificationServiceInstance.sendVerificationCode(),
    verifyCode: (code) => verificationServiceInstance.verifyCode(code),
    isUserVerified: () => verificationServiceInstance.isUserVerified(),
    resendVerificationCode: () => verificationServiceInstance.resendVerificationCode()
};

export async function getVerificationStatus() {
    return verificationServiceInstance.getVerificationStatus();
}

export async function sendVerificationCode() {
    return verificationServiceInstance.sendVerificationCode();
}

export async function verifyCode(code) {
    return verificationServiceInstance.verifyCode(code);
}

export async function isUserVerified() {
    return verificationServiceInstance.isUserVerified();
}

export async function resendVerificationCode() {
    return verificationServiceInstance.resendVerificationCode();
}

// Instance VerificationService
export { VerificationService }; 