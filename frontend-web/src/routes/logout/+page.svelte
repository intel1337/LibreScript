<script>
    import { goto } from '$app/navigation';
    import { logout } from '$lib/services/loginService.js';
    import { onMount } from 'svelte';

    onMount(() => {

        logout();
        console.log('User logged out successfully');
        

        goto('/login');
    });
</script>

<div class="logout-container">
    <div class="logout-message">
        <h2>Logout in progress...</h2>
        <div class="spinner"></div>
    </div>
</div>

<style>
.logout-container {
    min-height: 100vh;
    display: flex;
    align-items: center;
    justify-content: center;
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    font-family: 'Inter', 'Roboto', Arial, sans-serif;
}

.logout-message {
    text-align: center;
    color: white;
}

.logout-message h2 {
    margin-bottom: 20px;
    font-weight: 600;
}

.spinner {
    width: 40px;
    height: 40px;
    border: 4px solid rgba(255, 255, 255, 0.3);
    border-top: 4px solid white;
    border-radius: 50%;
    animation: spin 1s linear infinite;
    margin: 0 auto;
}

@keyframes spin {
    0% { transform: rotate(0deg); }
    100% { transform: rotate(360deg); }
}
</style>