<script>
import { goto } from '$app/navigation';
import { register, getCurrentUser, isLoggedIn, logout } from '$lib/services/loginService.js';
import { onMount } from 'svelte';

let username = '';
let password = '';
let fullName = '';
let email = '';
let error = '';
let loading = false;
let checking = true;
let acceptTerms = false;

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

async function handleRegister(e) {
    e.preventDefault();
    if (!username.trim() || !password.trim() || !fullName.trim() || !email.trim()) {
        error = 'Please fill in all fields.';
        return;
    }
    
    if (!acceptTerms) {
        error = 'You must accept the Terms of Service to create an account.';
        return;
    }
    
    error = '';
    loading = true;
    
    try {
        const result = await register({ username, fullName, password, email });
        if (result.success) {
            const user = await getCurrentUser();
            if (user) {
                console.log('Registration successful, redirecting to home');
                goto('/');
            } else {
                error = 'Token validation error. Please try again.';
                logout();
            }
        } else {
            error = result.error || 'Unknown registration error.';
        }
    } catch (err) {
        console.error('Registration error:', err);
        error = 'Registration error. Please try again.';
    } finally {
        loading = false;
    }
}
</script>

{#if checking}
    <div class="checking-container">
        <div class="spinner"></div>
        <p>Checking connection...</p>
    </div>
{:else}
    <section class="register-page">
        <div class="register-container">
            <div class="register-header">
                <h1>Register</h1>
                <p>Create your account</p>
            </div>
            
            <form on:submit|preventDefault={handleRegister}>
                <div class="form-group">
                    <label for="fullName">Full Name</label>
                    <input 
                        id="fullName" 
                        type="text" 
                        bind:value={fullName} 
                        required 
                        autocomplete="name"
                        placeholder="Enter your full name"
                        disabled={loading}
                    />
                </div>
                
                <div class="form-group">
                    <label for="username">Username</label>
                    <input 
                        id="username" 
                        type="text" 
                        bind:value={username} 
                        required 
                        autocomplete="username"
                        placeholder="Choose a username"
                        disabled={loading}
                    />
                </div>
                
                <div class="form-group">
                    <label for="email">Email address</label>
                    <input 
                        id="email" 
                        type="email" 
                        bind:value={email} 
                        required 
                        autocomplete="email"
                        placeholder="Enter your email address"
                        disabled={loading}
                    />
                </div>
                
                <div class="form-group">
                    <label for="password">Password</label>
                    <input 
                        id="password" 
                        type="password" 
                        bind:value={password} 
                        required 
                        autocomplete="new-password"
                        placeholder="Create a secure password"
                        disabled={loading}
                    />
                </div>
                
                <div class="form-group terms-group">
                    <div class="checkbox-container">
                        <input 
                            id="acceptTerms" 
                            type="checkbox" 
                            bind:checked={acceptTerms}
                            disabled={loading}
                        />
                        <label for="acceptTerms" class="checkbox-label">
                            I agree to the <a href="/tos" target="_blank" rel="noopener noreferrer">Terms of Service</a> 
                            and <a href="/tos#privacy" target="_blank" rel="noopener noreferrer">Privacy Policy</a>
                        </label>
                    </div>
                </div>
                
                {#if error}
                    <div class="error">{error}</div>
                {/if}
                
                <button type="submit" disabled={loading || !username.trim() || !password.trim() || !fullName.trim() || !email.trim() || !acceptTerms}>
                    {loading ? 'Register...' : 'Register'}
                </button>
                
                <div class="form-footer">
                    <p>Already have an account? <a href="/login">Login</a></p>
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

.register-page {
    min-height: 100vh;
    display: flex;
    align-items: center;
    justify-content: center;

    padding: 20px;
    box-sizing: border-box;
}

.register-container {
    width: 100%;
    max-width: 480px;
    background: #fff;
    border-radius: 16px;
    box-shadow: 0 10px 40px rgba(0,0,0,0.15);
    padding: 40px;
    text-align: center;
    font-family: 'Inter', 'Roboto', Arial, sans-serif;
    box-sizing: border-box;
}

.register-header {
    margin-bottom: 32px;
}

.register-header h1 {
    font-size: 2.25rem;
    margin: 0 0 8px 0;
    color: #212529;
    font-weight: 700;
}

.register-header p {
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
input[type="password"],
input[type="email"] {
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

/* Terms of Service Checkbox Styles */
.terms-group {
    margin: 24px 0 !important;
}

.checkbox-container {
    display: flex;
    align-items: flex-start;
    gap: 12px;
    padding: 16px;
    background: #f8f9fa;
    border: 2px solid #e9ecef;
    border-radius: 10px;
    transition: all 0.2s ease;
}

.checkbox-container:hover {
    border-color: #0969da;
    background: #f6f8fa;
}

input[type="checkbox"] {
    width: 18px;
    height: 18px;
    margin: 0;
    cursor: pointer;
    flex-shrink: 0;
    margin-top: 2px;
    accent-color: #0969da;
}

input[type="checkbox"]:disabled {
    cursor: not-allowed;
    opacity: 0.6;
}

.checkbox-label {
    font-size: 0.9rem;
    color: #495057;
    line-height: 1.5;
    cursor: pointer;
    margin: 0;
    font-weight: 400;
}

.checkbox-label a {
    color: #0969da;
    text-decoration: none;
    font-weight: 600;
    transition: color 0.2s ease;
}

.checkbox-label a:hover {
    color: #0850b3;
    text-decoration: underline;
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
    .register-page {
        padding: 15px;
    }
    
    .register-container {
        padding: 32px 24px;
        max-width: 100%;
    }
    
    .register-header h1 {
        font-size: 2rem;
    }
    
    .register-header p {
        font-size: 0.95rem;
    }
    
    input[type="text"],
    input[type="password"],
    input[type="email"] {
        padding: 12px 14px;
        font-size: 0.95rem;
    }
    
    button[type="submit"] {
        padding: 12px 0;
        font-size: 1rem;
    }
}

@media (max-width: 480px) {
    .register-page {
        padding: 10px;
    }
    
    .register-container {
        padding: 24px 20px;
    }
    
    .register-header {
        margin-bottom: 28px;
    }
    
    .register-header h1 {
        font-size: 1.75rem;
    }
    
    form {
        gap: 16px;
    }
    
    .form-group {
        gap: 6px;
    }
    
    input[type="text"],
    input[type="password"],
    input[type="email"] {
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
