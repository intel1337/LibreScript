// src/lib/services/registerService.js
const API_URL = 'http://192.168.10.106:5028';


export async function register({ username, fullName, password, email }) {
    try {
        const res = await fetch(`${API_URL}/api/user/register/`, {
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
    } catch {
        return { error: 'Erreur réseau ou serveur.' };
    }
}
