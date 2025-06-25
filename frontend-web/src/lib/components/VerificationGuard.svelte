<script>
    import { onMount } from 'svelte';
    import { goto } from '$app/navigation';
    import { verificationService } from '$lib/services/verificationService';
    
    let loading = true;
    let verified = false;
    let error = '';
    
    onMount(async () => {
        try {
            const status = await verificationService.getVerificationStatus();
            verified = status.verified;
            
            if (!verified) {
                console.log('User not verified, redirecting to verification page');
                goto('/verify');
                return;
            }
            
            console.log('User is verified');
        } catch (err) {
            console.error('Verification guard error:', err);
            error = err.message;
            // Si erreur de token, laisser AuthGuard gérer
            if (err.message.includes('Token')) {
                verified = true; // Laisser passer pour que AuthGuard redirige
            } else {
                goto('/verify');
            }
        } finally {
            loading = false;
        }
    });
</script>

{#if loading}
    <div class="verification-loading">
        <div class="spinner"></div>
        <p>Vérification du compte...</p>
    </div>
{:else if verified}
    <slot />
{:else if error}
    <div class="verification-error">
        <p>Erreur lors de la vérification: {error}</p>
    </div>
{/if}

<style>
.verification-loading, .verification-error {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    min-height: 50vh;
    color: #666;
    font-family: 'Inter', 'Roboto', Arial, sans-serif;
}

.verification-error {
    color: #d63384;
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