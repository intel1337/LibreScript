// src/lib/services/authService.js
import { goto } from '$app/navigation';
import { API_BASE_URL } from '$lib/config.js';



export async function checkAuth() {
    const token = localStorage.getItem('token');
    if (!token) {
        goto('/login');
        return null;
    }
    try {
        const res = await fetch(`${API_BASE_URL}/api/user/authenticate`, {
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
