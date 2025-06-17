const API_URL = import.meta.env.VITE_API_URL;


export function isLoggedIn() {
    if (typeof localStorage === 'undefined') return false;
    const token = localStorage.getItem('token');
    return !!token && !isTokenExpired(token);
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
}

/**
 * Décodes un JWT token (sans vérification de signature)
 * @param {string} token 
 * @returns {object|null}
 */
function decodeJWT(token) {
    try {
        if (!token || token.split('.').length !== 3) {
            return null;
        }
        const base64Url = token.split('.')[1];
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        const jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));
        return JSON.parse(jsonPayload);
    } catch (error) {
        console.error('Error decoding JWT:', error);
        return null;
    }
}


function isTokenExpired(token) {
    const decoded = decodeJWT(token);
    if (!decoded || !decoded.exp) return true;
    
    const now = Math.floor(Date.now() / 1000);
    return now > decoded.exp;
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


    if (isTokenExpired(token)) {
        console.log('Token is expired');
        logout();
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
        const response = await fetch(`${API_URL}/api/user/login`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ username, password })
        });

        console.log('Login response status:', response.status);
        const data = await response.json();
        console.log('Login response data:', data);

        if (response.ok && data.token) {
            console.log('Login successful, storing token');
            storeToken(data.token);
            return { success: true, token: data.token };
        } else {
            console.log('Login failed:', data.message || 'Unknown error');
            return { success: false, error: data.message || 'Unknown error' };
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
