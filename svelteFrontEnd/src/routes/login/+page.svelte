<script>
import { goto } from '$app/navigation';
import { auth } from '$lib/services/loginService.js';

// Supprime le token à l'arrivée sur la page login (uniquement côté client)
if (typeof window !== 'undefined') {
    localStorage.removeItem('token');
}

let username = '';
let password = '';
let error = '';
let loading = false;

async function handleLogin(e) {
    e.preventDefault();
    error = '';
    loading = true;
    const result = await auth(username, password);
    if (result.token) {
        localStorage.setItem('token', result.token); // Stocke le token JWT
        goto('/');
    } else {
        error = result.error || 'Erreur inconnue.';
    }
    loading = false;
}
</script>

<section class="login-page">
    <h1>Connexion</h1>
    <form on:submit|preventDefault={handleLogin}>
        <div class="form-group">
            <label for="username">Nom d'utilisateur</label>
            <input id="username" type="text" bind:value={username} required autocomplete="username" />
        </div>
        <div class="form-group">
            <label for="password">Mot de passe</label>
            <input id="password" type="password" bind:value={password} required autocomplete="current-password" />
        </div>
        {#if error}
            <div class="error">{error}</div>
        {/if}
        <button type="submit" disabled={loading}>{loading ? 'Connexion...' : 'Se connecter'}</button>
    </form>
</section>

<style>
.login-page {
    max-width: 400px;
    margin: 3rem auto;
    background: #fff;
    border-radius: 12px;
    box-shadow: 0 2px 16px rgba(0,0,0,0.07);
    padding: 2rem;
    text-align: center;
    font-family: 'Roboto', Arial, sans-serif;
    box-sizing: border-box;
}
.login-page h1 {
    font-size: 2rem;
    margin-bottom: 1.5rem;
    color: #333;
}
form {
    display: flex;
    flex-direction: column;
    gap: 1.2rem;
    align-items: center; /* Ajouté pour centrer les inputs */
}
.form-group {
    display: flex;
    flex-direction: column;
    align-items: center; /* Ajouté pour centrer le label + input */
    width: 100%;
}
label {
    font-weight: 500;
    margin-bottom: 0.3rem;
    color: #444;
}
input[type="text"],
input[type="password"] {
    width: 100%;
    max-width: 340px;
    box-sizing: border-box;
    padding: 0.9rem 1.2rem;
    border: 1px solid #ccc;
    border-radius: 10px;
    font-size: 1.05rem;
    background: #fafafa;
    transition: border 0.2s;
    margin: 0 auto;
    display: block;
}
input:focus {
    border: 1.5px solid #888;
    outline: none;
}
button[type="submit"] {
    background: #333;
    color: #fff;
    border: none;
    border-radius: 12px; /* Plus arrondi */
    padding: 0.9rem 0;
    font-size: 1rem; /* Moins grand */
    font-weight: 600;
    cursor: pointer;
    transition: background 0.2s;
    width: 100%;
    max-width: 340px;
    margin: 0 auto;
}
button[disabled] {
    opacity: 0.7;
    cursor: not-allowed;
}
button[type="submit"]:hover:not([disabled]) {
    background: #222;
}
.error {
    color: #c00;
    background: #ffeaea;
    border-radius: 5px;
    padding: 0.5rem 1rem;
    font-size: 0.98rem;
}
</style>
