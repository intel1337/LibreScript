import { updateAuthState, clearAuthState } from '$lib/stores/authStore.js';

const API_URL = 'http://localhost:5028';


export function isLoggedIn() {
    if (typeof localStorage === 'undefined') return false;
    const token = localStorage.getItem('token');
    return !!token;
}


export function storeToken(token) {
    if (typeof localStorage !== 'undefined' && token) {
        console.log('Storing token:', token);
        localStorage.setItem('token', token);
        console.log('Token stored successfully');
    } else {
        console.log('Cannot store token:', { hasLocalStorage: typeof localStorage !== 'undefined', hasToken: !!token });
    }
}

/**
 * Récupère le token stocké
 * @returns {string|null}
 */
export function getStoredToken() {
    if (typeof localStorage === 'undefined') {
        console.log('localStorage is not available');
        return null;
    }
    const token = localStorage.getItem('token');
    console.log('Retrieved token:', token);
    return token;
}

/**
 * Déconnecte l'utilisateur (supprime le token)
 */
export function logout() {
    if (typeof localStorage !== 'undefined') {
        localStorage.removeItem('token');
        console.log('User logged out, token removed');
    }
    // Clear auth state
    clearAuthState();
}




async function validateTokenWithServer(token) {
    try {
        console.log('Validating token:', token);
        const res = await fetch(`${API_URL}/api/user/authenticate`, {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
            }
        });

        console.log('Validation response status:', res.status);
        if (res.ok) {
            const userData = await res.json();
            console.log('Validation successful, user data:', userData);
            return { valid: true, userData };
        } else {
            const errorText = await res.text();
            console.log('Server token validation failed:', res.status, errorText);
            return { valid: false };
        }
    } catch (error) {
        console.error('Error validating token with server:', error);
        return { valid: false };
    }
}


export async function getCurrentUser() {
    const token = getStoredToken();
    
    if (!token) {
        console.log('No token found');
        return null;
    }




    const validation = await validateTokenWithServer(token);
    
    if (validation.valid && validation.userData) {
        console.log('Token valid, user data received:', validation.userData);
        return validation.userData;
    } else {
        console.log('Token invalid, removing it');
        logout();
        return null;
    }
}

/**
 * Authentifie l'utilisateur via l'API
 * @param {string} username
 * @param {string} password
 * @returns {Promise<{success: boolean, token?: string, error?: string}>}
 */
export async function login(username, password) {
    try {
        console.log('Attempting login for user:', username);
        const response = await fetch(`${API_URL}/api/user/login/`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ username, password })
        });

        console.log('Login response status:', response.status);
        
        if (response.ok) {
            const data = await response.json();
            console.log('Login response data:', data);
            
            if (data.token) {
                console.log('Login successful, storing token');
                storeToken(data.token);
                // Update auth state
                const user = await getCurrentUser();
                updateAuthState(true, user);
                return { success: true, token: data.token };
            } else {
                return { success: false, error: 'No token received' };
            }
        } else {
            // Handle non-JSON error responses
            let errorMessage;
            const contentType = response.headers.get('content-type');
            
            if (contentType && contentType.includes('application/json')) {
                try {
                    const data = await response.json();
                    errorMessage = data.message || 'Login failed';
                } catch {
                    errorMessage = 'Login failed';
                }
            } else {
                try {
                    const text = await response.text();
                    errorMessage = text || 'Login failed';
                } catch {
                    errorMessage = 'Login failed';
                }
            }
            console.log('Login failed:', errorMessage);
            return { success: false, error: errorMessage };
        }
    } catch (error) {
        console.error('Login error:', error);
        return { success: false, error: 'Erreur réseau ou serveur.' };
    }
}

/**
 * Inscrit un nouvel utilisateur via l'API
 * @param {Object} user - { username, fullName, password, email }
 * @returns {Promise<{success: boolean, token?: string, error?: string}>}
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

                storeToken(data.token);
                return { success: true, token: data.token };
            } else {
                return { success: false, error: 'Token manquant dans la réponse.' };
            }
        } else {
            const msg = await res.text();
            return { success: false, error: msg || 'Erreur lors de l\'inscription.' };
        }
    } catch (error) {
        console.error('Register error:', error);
        return { success: false, error: 'Erreur réseau ou serveur.' };
    }
}

/**

 */
export async function requireAuth() {
    const user = await getCurrentUser();
    return !!user;
}


export async function auth(username, password) {
    const result = await login(username, password);
    return result.success ? { token: result.token } : { error: result.error };
}
