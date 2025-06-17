import { writable } from 'svelte/store';

// Store to signal when posts should be refreshed
export const refreshPosts = writable(0);

// Function to trigger a refresh
export function triggerPostsRefresh() {
    refreshPosts.update(n => n + 1);
} 