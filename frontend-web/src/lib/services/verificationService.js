const API_BASE_URL = 'http://192.168.10.106:5028/api';

export const verificationService = {
    async getVerificationStatus() {
        try {
            const token = localStorage.getItem('token');
            if (!token) {
                throw new Error('Token non trouvé');
            }

            const response = await fetch(`${API_BASE_URL}/verification/status`, {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json',
                },
            });

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.error || 'Erreur lors de la vérification du statut');
            }

            return await response.json();
        } catch (error) {
            console.error('Erreur getVerificationStatus:', error);
            throw error;
        }
    },

    async sendVerificationCode() {
        try {
            const token = localStorage.getItem('token');
            if (!token) {
                throw new Error('Token non trouvé');
            }

            const response = await fetch(`${API_BASE_URL}/verification/send-code`, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json',
                },
            });

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.error || 'Erreur lors de l\'envoi du code');
            }

            return await response.json();
        } catch (error) {
            console.error('Erreur sendVerificationCode:', error);
            throw error;
        }
    },

    async verifyCode(code) {
        try {
            const token = localStorage.getItem('token');
            if (!token) {
                throw new Error('Token non trouvé');
            }

            const response = await fetch(`${API_BASE_URL}/verification/verify`, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ code })
            });

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.error || 'Erreur lors de la vérification du code');
            }

            return await response.json();
        } catch (error) {
            console.error('Erreur verifyCode:', error);
            throw error;
        }
    }
}; 