// src/lib/services/loginService.js


const API_URL = import.meta.env.VITE_API_URL;

/**
 * Vérifie si l'utilisateur est connecté (présence d'un token dans le localStorage)
 * @returns {boolean}
 */
export function isLoggedIn() {
    return typeof localStorage !== 'undefined' && !!localStorage.getItem('token');
}

/**
 * Déconnecte l'utilisateur (supprime le token)
 */
export function logout() {
    if (typeof localStorage !== 'undefined') {
        localStorage.removeItem('token');
    }
}

/**
 * Authentifie l'utilisateur via l'API et retourne la réponse (token ou erreur)
 * @param {string} username
 * @param {string} password
 * @returns {Promise<{token?: string, error?: string}>}
 */
export async function auth(username, password) {
    try {
        const res = await fetch(`${API_URL}/api/user/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ username, password })
        });
        if (res.ok) {
            const data = await res.json();
            if (data.token) {
                return { token: data.token };
            } else {
                return { error: 'Token manquant dans la réponse.' };
            }
        } else {
            const msg = await res.text();
            return { error: msg || 'Identifiants invalides.' };
        }
    } catch {
        return { error: 'Erreur réseau ou serveur.' };
    }
}

/**
 * Inscrit un nouvel utilisateur via l'API REST
 * @param {Object} user - { username, fullName, password, email }
 * @returns {Promise<{token?: string, error?: string}>}
 */
export async function register({ username, fullName, password, email }) {
    try {
        const res = await fetch(`${API_URL}/api/user/register`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ username, fullName, password, email })
        });
        if (res.ok) {
            const data = await res.json();
            if (data.token) {
                return { token: data.token };
            } else {
                return { error: 'Token manquant dans la réponse.' };
            }
        } else {
            const msg = await res.text();
            return { error: msg || 'Erreur lors de l\'inscription.' };
        }
    } catch {
        return { error: 'Erreur réseau ou serveur.' };
    }
}
