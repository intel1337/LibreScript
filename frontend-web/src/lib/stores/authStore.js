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