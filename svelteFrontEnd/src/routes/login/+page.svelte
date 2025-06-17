<script>
import { goto } from '$app/navigation';
import { login, getCurrentUser, isLoggedIn, logout } from '$lib/services/loginService.js';
import { onMount } from 'svelte';

let username = '';
let password = '';
let error = '';
let loading = false;
let checking = true;

// Check if user is already logged in
onMount(async () => {
    try {
        if (isLoggedIn()) {
            const user = await getCurrentUser();
            if (user) {
                console.log('User already logged in, redirecting to home');
                goto('/');
                return;
            }
        }
    } catch (error) {
        console.log('Error checking authentication status:', error);
    } finally {
        checking = false;
    }
});

async function handleLogin(e) {
    e.preventDefault();
    if (!username.trim() || !password.trim()) {
        error = 'Veuillez remplir tous les champs.';
        return;
    }
    
    error = '';
    loading = true;
    
    try {
        const result = await login(username, password);
        if (result.success) {
            // Vérifier que le token est valide avant de rediriger
            const user = await getCurrentUser();
            if (user) {
                console.log('Login successful, redirecting to home');
                goto('/');
            } else {
                error = 'Erreur de validation du token. Veuillez réessayer.';
                logout();
            }
        } else {
            error = result.error || 'Erreur de connexion inconnue.';
        }
    } catch (err) {
        console.error('Login error:', err);
        error = 'Erreur de connexion. Veuillez réessayer.';
    } finally {
        loading = false;
    }
}
</script>

{#if checking}
    <div class="checking-container">
        <div class="spinner"></div>
        <p>Vérification de la connexion...</p>
    </div>
{:else}
    <section class="login-page">
        <div class="login-container">
            <div class="login-header">
                <h1>Connexion</h1>
                <p>Connectez-vous à votre compte LibreScript</p>
            </div>
            
            <form on:submit|preventDefault={handleLogin}>
                <div class="form-group">
                    <label for="username">Nom d'utilisateur</label>
                    <input 
                        id="username" 
                        type="text" 
                        bind:value={username} 
                        required 
                        autocomplete="username"
                        placeholder="Entrez votre nom d'utilisateur"
                        disabled={loading}
                    />
                </div>
                
                <div class="form-group">
                    <label for="password">Mot de passe</label>
                    <input 
                        id="password" 
                        type="password" 
                        bind:value={password} 
                        required 
                        autocomplete="current-password"
                        placeholder="Entrez votre mot de passe"
                        disabled={loading}
                    />
                </div>
                
                {#if error}
                    <div class="error">{error}</div>
                {/if}
                
                <button type="submit" disabled={loading || !username.trim() || !password.trim()}>
                    {loading ? 'Connexion...' : 'Se connecter'}
                </button>
                
                <div class="form-footer">
                    <p>Pas encore de compte ? <a href="/register">S'inscrire</a></p>
                </div>
            </form>
        </div>
    </section>
{/if}

<style>
.checking-container {
    min-height: 100vh;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    padding: 20px;
    color: white;
    font-family: 'Inter', 'Roboto', Arial, sans-serif;
}

.spinner {
    width: 40px;
    height: 40px;
    border: 4px solid rgba(255, 255, 255, 0.3);
    border-top: 4px solid white;
    border-radius: 50%;
    animation: spin 1s linear infinite;
    margin-bottom: 16px;
}

@keyframes spin {
    0% { transform: rotate(0deg); }
    100% { transform: rotate(360deg); }
}

.login-page {
    min-height: 100vh;
    display: flex;
    align-items: center;
    justify-content: center;

    padding: 20px;
    box-sizing: border-box;
}

.login-container {
    width: 100%;
    max-width: 450px;
    background: #fff;
    border-radius: 16px;
    box-shadow: 0 10px 40px rgba(0,0,0,0.15);
    padding: 40px;
    text-align: center;
    font-family: 'Inter', 'Roboto', Arial, sans-serif;
    box-sizing: border-box;
}

.login-header {
    margin-bottom: 32px;
}

.login-header h1 {
    font-size: 2.25rem;
    margin: 0 0 8px 0;
    color: #212529;
    font-weight: 700;
}

.login-header p {
    font-size: 1rem;
    color: #6c757d;
    margin: 0;
}

form {
    display: flex;
    flex-direction: column;
    gap: 20px;
    text-align: left;
}

.form-group {
    display: flex;
    flex-direction: column;
    gap: 8px;
}

label {
    font-weight: 600;
    color: #495057;
    font-size: 0.95rem;
}

input[type="text"],
input[type="password"] {
    width: 100%;
    box-sizing: border-box;
    padding: 14px 16px;
    border: 2px solid #e9ecef;
    border-radius: 10px;
    font-size: 1rem;
    background: #fff;
    transition: all 0.2s ease;
    font-family: inherit;
}

input:focus {
    border-color: #0969da;
    outline: none;
    box-shadow: 0 0 0 3px rgba(9, 105, 218, 0.1);
}

input::placeholder {
    color: #adb5bd;
}

button[type="submit"] {
    background: linear-gradient(135deg, #0969da 0%, #0850b3 100%);
    color: #fff;
    border: none;
    border-radius: 10px;
    padding: 14px 0;
    font-size: 1.05rem;
    font-weight: 600;
    cursor: pointer;
    transition: all 0.2s ease;
    width: 100%;
    margin-top: 8px;
}

button[disabled] {
    opacity: 0.7;
    cursor: not-allowed;
    transform: none;
}

button[type="submit"]:hover:not([disabled]) {
    background: linear-gradient(135deg, #0850b3 0%, #0969da 100%);
    transform: translateY(-2px);
    box-shadow: 0 6px 20px rgba(9, 105, 218, 0.3);
}

.error {
    color: #dc3545;
    background: #f8d7da;
    border: 1px solid #f5c6cb;
    border-radius: 8px;
    padding: 12px 16px;
    font-size: 0.9rem;
    text-align: center;
}

.form-footer {
    text-align: center;
    margin-top: 24px;
    padding-top: 20px;
    border-top: 1px solid #e9ecef;
}

.form-footer p {
    margin: 0;
    color: #6c757d;
    font-size: 0.95rem;
}

.form-footer a {
    color: #0969da;
    text-decoration: none;
    font-weight: 600;
}

.form-footer a:hover {
    text-decoration: underline;
}

/* Mobile Responsive Design */
@media (max-width: 768px) {
    .login-page {
        padding: 15px;
    }
    
    .login-container {
        padding: 32px 24px;
        max-width: 100%;
    }
    
    .login-header h1 {
        font-size: 2rem;
    }
    
    .login-header p {
        font-size: 0.95rem;
    }
    
    input[type="text"],
    input[type="password"] {
        padding: 12px 14px;
        font-size: 0.95rem;
    }
    
    button[type="submit"] {
        padding: 12px 0;
        font-size: 1rem;
    }
}

@media (max-width: 480px) {
    .login-page {
        padding: 10px;
    }
    
    .login-container {
        padding: 24px 20px;
    }
    
    .login-header {
        margin-bottom: 28px;
    }
    
    .login-header h1 {
        font-size: 1.75rem;
    }
    
    form {
        gap: 16px;
    }
    
    .form-group {
        gap: 6px;
    }
    
    input[type="text"],
    input[type="password"] {
        padding: 10px 12px;
        font-size: 0.9rem;
    }
    
    button[type="submit"] {
        padding: 12px 0;
        font-size: 0.95rem;
    }
    
    .form-footer {
        margin-top: 20px;
        padding-top: 16px;
    }
    
    .form-footer p {
        font-size: 0.9rem;
    }
}
</style>
