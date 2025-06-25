<script>
    import { onMount } from 'svelte';
    import { verificationService } from '$lib/services/verificationService';
    import { goto } from '$app/navigation';

    let code = '';
    let error = '';
    let success = '';
    let loading = false;
    let isVerified = false;
    let statusMessage = '';

    onMount(async () => {
        // Vérifier le statut de vérification au chargement
        try {
            const status = await verificationService.getVerificationStatus();
            isVerified = status.verified;
            statusMessage = status.message;
            
            if (isVerified) {
                success = statusMessage;
                goto('/');
            }
        } catch (err) {
            error = err.message;
            if (err.message.includes('Token')) {
                goto('/login');
            }
        }
    });

    async function handleSubmit() {
        if (code.length !== 6) {
            error = 'Le code doit contenir exactement 6 chiffres';
            return;
        }

        loading = true;
        error = '';
        success = '';

        try {
            const result = await verificationService.verifyCode(code);
            success = result.message;
            isVerified = true;
            
            setTimeout(() => {
                goto('/');
            }, 2000);
        } catch (err) {
            error = err.message;
        } finally {
            loading = false;
        }
    }

    async function sendNewCode() {
        loading = true;
        error = '';
        success = '';

        try {
            const result = await verificationService.sendVerificationCode();
            success = result.message;
        } catch (err) {
            error = err.message;
        } finally {
            loading = false;
        }
    }

    function handleInput(event) {
        // Ne permettre que les chiffres et limiter à 6 caractères
        const value = event.target.value.replace(/\D/g, '').slice(0, 6);
        code = value;
        event.target.value = value;
    }
</script>

<svelte:head>
    <title>Vérification de compte - LibreScript</title>
</svelte:head>

<div class="verify-container">
    <div class="verify-card">
        <h1>Vérification de compte</h1>
        
        {#if isVerified}
            <div class="success-message">
                <svg width="48" height="48" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                    <path d="M9 12L11 14L15 10M21 12C21 16.9706 16.9706 21 12 21C7.02944 21 3 16.9706 3 12C3 7.02944 7.02944 3 12 3C16.9706 3 21 7.02944 21 12Z" stroke="#10B981" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                </svg>
                <p>{statusMessage}</p>
                <p><small>Vous allez être redirigé automatiquement...</small></p>
            </div>
        {:else}
            <p class="description">
                Entrez le code de vérification à 6 chiffres envoyé à votre adresse email.
            </p>

            <form on:submit|preventDefault={handleSubmit}>
                <div class="code-input-container">
                    <input 
                        type="text" 
                        bind:value={code}
                        on:input={handleInput}
                        placeholder="000000"
                        maxlength="6"
                        class="code-input"
                        disabled={loading}
                    />
                </div>

                {#if error}
                    <div class="error-message">
                        {error}
                    </div>
                {/if}

                {#if success}
                    <div class="success-message">
                        {success}
                    </div>
                {/if}

                <button 
                    type="submit" 
                    class="verify-btn"
                    disabled={loading || code.length !== 6}
                >
                    {#if loading}
                        Vérification...
                    {:else}
                        Vérifier
                    {/if}
                </button>
            </form>

            <div class="resend-section">
                <p>Vous n'avez pas reçu le code ?</p>
                <button 
                    type="button" 
                    class="resend-btn"
                    on:click={sendNewCode}
                    disabled={loading}
                >
                    {#if loading}
                        Envoi...
                    {:else}
                        Renvoyer le code
                    {/if}
                </button>
            </div>
        {/if}
    </div>
</div>

<style>
    *{
        font-family: 'Roboto', sans-serif;

    }
    .verify-container {
        display: flex;
        justify-content: center;
        align-items: center;
        min-height: 100vh;
        background-color: #f5f5f5;
        padding: 20px;
    }

    .verify-card {
        background: white;
        border-radius: 10px;
        padding: 40px;
        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        max-width: 400px;
        width: 100%;
        text-align: center;
    }

    h1 {
        color: #333;
        margin-bottom: 20px;
        font-size: 2rem;
    }

    .description {
        color: #666;
        margin-bottom: 30px;
        line-height: 1.5;
    }

    .code-input-container {
        margin-bottom: 20px;
    }

    .code-input {
        width: 100%;
        padding: 15px;
        font-size: 2rem;
        text-align: center;
        border: 2px solid #ddd;
        border-radius: 8px;
        letter-spacing: 0.5rem;
        font-weight: bold;
        color: #333;
        transition: border-color 0.2s ease;
    }

    .code-input:focus {
        outline: none;
        border-color: #007bff;
    }

    .code-input:disabled {
        background-color: #f5f5f5;
        cursor: not-allowed;
    }

    .verify-btn {
        width: 100%;
        padding: 15px;
        background-color: #007bff;
        color: white;
        border: none;
        border-radius: 8px;
        font-size: 1.1rem;
        font-weight: bold;
        cursor: pointer;
        transition: background-color 0.2s ease;
        margin-bottom: 20px;
    }

    .verify-btn:hover:not(:disabled) {
        background-color: #0056b3;
    }

    .verify-btn:disabled {
        background-color: #ccc;
        cursor: not-allowed;
    }

    .resend-section {
        border-top: 1px solid #eee;
        padding-top: 20px;
    }

    .resend-section p {
        color: #666;
        margin-bottom: 10px;
    }

    .resend-btn {
        background: none;
        border: none;
        color: #007bff;
        text-decoration: underline;
        cursor: pointer;
        font-size: 1rem;
    }

    .resend-btn:hover:not(:disabled) {
        color: #0056b3;
    }

    .resend-btn:disabled {
        color: #ccc;
        cursor: not-allowed;
    }

    .error-message {
        background-color: #f8d7da;
        color: #721c24;
        padding: 10px;
        border-radius: 5px;
        margin-bottom: 15px;
        border: 1px solid #f5c6cb;
    }

    .success-message {
        background-color: #d4edda;
        color: #155724;
        padding: 20px;
        border-radius: 5px;
        margin-bottom: 15px;
        border: 1px solid #c3e6cb;
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: 10px;
    }

    .success-message svg {
        margin-bottom: 10px;
    }
</style> 