<script>
    import { onMount } from 'svelte';
    import { goto } from '$app/navigation';
    import { getCurrentUser, isLoggedIn } from '$lib/services/loginService.js';
    
    export let redirectTo = '/login';
    
    let loading = true;
    let authenticated = false;
    
    onMount(async () => {
        try {
            if (!isLoggedIn()) {
                console.log('Not logged in, redirecting to login');
                goto(redirectTo);
                return;
            }
            
            const user = await getCurrentUser();
            if (user) {
                authenticated = true;
                console.log('User authenticated:', user);
            } else {
                console.log('Authentication failed, redirecting to login');
                goto(redirectTo);
            }
        } catch (error) {
            console.error('Auth guard error:', error);
            goto(redirectTo);
        } finally {
            loading = false;
        }
    });
</script>

{#if loading}
    <div class="auth-loading">
        <div class="spinner"></div>
        <p>Vérification de l'authentification...</p>
    </div>
{:else if authenticated}
    <slot />
{/if}

<style>
.auth-loading {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    min-height: 50vh;
    color: #666;
    font-family: 'Inter', 'Roboto', Arial, sans-serif;
}

.spinner {
    width: 32px;
    height: 32px;
    border: 3px solid #f3f3f3;
    border-top: 3px solid #0969da;
    border-radius: 50%;
    animation: spin 1s linear infinite;
    margin-bottom: 12px;
}

@keyframes spin {
    0% { transform: rotate(0deg); }
    100% { transform: rotate(360deg); }
}
</style> 