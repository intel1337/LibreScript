// src/lib/services/registerService.js
const API_URL = import.meta.env.VITE_API_URL;

/**
 * Inscrit un nouvel utilisateur via l'API backend.
 * @param {Object} data - { username, fullName, password, email }
 * @returns {Promise<Object>} - { token } ou { error }
 */
export async function register({ username, fullName, password, email }) {
    try {
        const res = await fetch(`${API_URL}/api/user/register`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ username, fullName, password, email })
        });
        if (res.ok) {
            return await res.json(); // { token }
            
        } else {
            const err = await res.json();
            return { error: err.error || 'Erreur lors de l\'inscription.' };
        }
    } catch (error) {
        return { error: 'Erreur réseau ou serveur.' };
    }
}
