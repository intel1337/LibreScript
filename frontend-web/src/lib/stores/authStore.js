import { writable } from 'svelte/store';

// Create reactive stores for authentication state
export const isLoggedIn = writable(false);
export const currentUser = writable(null);

// Function to update authentication state
export function updateAuthState(loggedIn, user = null) {
    isLoggedIn.set(loggedIn);
    currentUser.set(user);
}

// Function to clear authentication state
export function clearAuthState() {
    isLoggedIn.set(false);
    currentUser.set(null);
}

// AuthStore object with methods expected by services
export const authStore = {
    // Set user and update logged in state
    setUser(user) {
        if (user) {
            currentUser.set(user);
            isLoggedIn.set(true);
        }
    },
    
    // Clear user and logged in state
    clearUser() {
        currentUser.set(null);
        isLoggedIn.set(false);
    },
    
    // Get current logged in state
    isLoggedIn() {
        let loggedIn = false;
        isLoggedIn.subscribe(value => loggedIn = value)();
        return loggedIn;
    },
    
    // Get current user
    getCurrentUser() {
        let user = null;
        currentUser.subscribe(value => user = value)();
        return user;
    }
}; 