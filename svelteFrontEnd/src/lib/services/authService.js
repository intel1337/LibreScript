// src/lib/services/authService.js
import { goto } from '$app/navigation';

const API_URL = import.meta.env.VITE_API_URL;

/**
 * Vérifie le JWT stocké dans le localStorage auprès de l'API backend.
 * Redirige vers /login si le token est absent ou invalide.
 * @returns {Promise<Object|null>} Les infos utilisateur si OK, sinon null.
 */
export async function checkAuth() {
    const token = localStorage.getItem('token');
    if (!token) {
        goto('/login');
        return null;
    }
    try {
        const res = await fetch(`${API_URL}/api/user/authenticate`, {
            method: 'POST',
            headers: { 'Authorization': `Bearer ${token}` }
        });
        if (res.ok) {
            return await res.json(); // infos user
        } else {
            localStorage.removeItem('token');
            goto('/login');
            return null;
        }
    } catch {
        localStorage.removeItem('token');
        goto('/login');
        return null;
    }
}
