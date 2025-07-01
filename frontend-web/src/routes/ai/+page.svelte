<script>
    import { onMount, tick } from 'svelte';
    import { aiService } from '$lib/services/aiService';
    import { marked } from 'marked';

    
    let userInput = '';
    let chatContainer;
    let error = '';
    let messages = [
        {
            type: 'ai',
            content: 'Hello, how can I help you today?'
        }
    ];

    function scrollToBottom() {
        if (chatContainer) {
            chatContainer.scrollTop = chatContainer.scrollHeight;
        }
    }

    async function handleSubmit() {
        if (userInput.trim() === '') return;
        

        messages = [...messages, {
            type: 'user',
            content: userInput
        }];
        
        const currentInput = userInput;
        userInput = '';
        
        // Wait for DOM update and scroll
        await tick();
        scrollToBottom();
        
        // Generate AI response
        try {
            const aiResponse = await aiService.askQuestion(currentInput);
            
            messages = [...messages, {
                type: 'ai',
                content: aiResponse
            }];
            
            // Wait for DOM update and scroll
            await tick();
            scrollToBottom();
        } catch (err) {
            console.error('Erreur lors de la génération de la réponse:', err);
            messages = [...messages, {
                type: 'ai',
                content: 'Désolé, j\'ai rencontré une erreur lors de la génération d\'une réponse. Veuillez réessayer.'
            }];
            
            // Wait for DOM update and scroll
            await tick();
            scrollToBottom();
        }
    }

    function handleKeyPress(event) {
        if (event.key === 'Enter') {
            handleSubmit();
        }
    }

    // Fonction pour convertir le markdown en HTML
    


    // Scroll to bottom on mount
    onMount(async () => {
        // Test de connexion au backend
        try {
            console.log('Test de connexion au backend...');
            const testResult = await aiService.testConnection();
            console.log('Connexion au backend réussie:', testResult);
        } catch (err) {
            console.error('Erreur de connexion au backend:', err);
            error = `Impossible de se connecter au backend: ${err.message}`;
        }
        scrollToBottom();
    });
</script>

<div class="ai-container">
    <div class="hero-container">

    </div>
    {#if error}
        <div class="ai-error-message">
            <p>
                {error}
            </p>
        </div>
    {/if}
    <div class="ai-chat-container" bind:this={chatContainer}>
        {#each messages as message}
            {#if message.type === 'ai'}
                <div class="ai-output-message">
                    <div class="message-header">
                        <strong>LibreScript AI:</strong>
                    </div>
                    <div class="message-content">
                        {@html aiService.renderMarkdown(message.content)}
                    </div>
                </div>
            {:else}
                <div class="ai-input-message">
                    <div class="message-header">
                        <strong>You:</strong>
                    </div>
                    <div class="message-content">
                        {message.content}
                    </div>
                </div>
            {/if}
        {/each}
    </div>

    <div class="ai-input-container">
        <input 
            id="ai-input" 
            type="text" 
            placeholder="Ask me anything..." 
            bind:value={userInput}
            on:keypress={handleKeyPress}
        />
        {#if userInput.length > 0 && !error}
        <button id="submit-btn" on:click={handleSubmit} aria-label="Submit">
            <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                <path d="M2 21L23 12L2 3V10L17 12L2 14V21Z" fill="currentColor"/>
            </svg>
        </button>
        {/if}
        {#if error}
            <button id="submit-btn" disabled on:click={handleSubmit} aria-label="Submit">
                <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                    <path d="M2 21L23 12L2 3V10L17 12L2 14V21Z" fill="currentColor"/>
                </svg>
            </button>
        {/if}
    </div>
</div>


<style>
    .ai-error-message {
        color: red;
        font-size: 1.2rem;
    }
    #ai-input {
        font-size: 1.2rem;
        border: none;
        text-decoration: none;
        outline: none;
        width: 100%;
        height: 100%;
        background-color: transparent;
        border-radius: 15px;
        padding: 0 15px;
    }
    
    #submit-btn {
        background-color: #007bff;
        border: none;
        border-radius: 50%;
        width: 40px;
        height: 40px;
        display: flex;
        align-items: center;
        justify-content: center;
        cursor: pointer;
        color: white;
        transition: background-color 0.2s ease;
        flex-shrink: 0;
        margin-left: 10px;
    }
    
    #submit-btn:hover {
        background-color: #0056b3;
    }
    
    #submit-btn:disabled {
        background-color: #ccc;
        cursor: not-allowed;
    }
    
    .ai-input-container {
        display: flex;
        flex-direction: row;
        align-items: center;
        justify-content: space-between;
        width: 80%;
        max-width: 600px;
        height: 50px;
        background-color: #f0f0f0;
        border-radius: 25px;
        padding: 5px;
    }

    *{
        font-family: 'Roboto', sans-serif;
    }
    .ai-container {
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
    }
    .hero-container {
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
    }
    .ai-chat-container {
        display: flex;
        flex-direction: column;


        overflow-y: auto;
        background-color: #f0f0f0;
        border-radius: 15px;
        width: 80%;
        height: 60vh;
        text-align: left;
        padding: 20px;
        margin-top: 20px;
        margin-bottom: 20px;
        font-size: 1.2rem;
        line-height: 1.5;
        color: #333;
    }
    .ai-output-message {
        display: flex;
        flex-direction: column;
        background-color: #ffffff;
        border-radius: 15px;
        width: auto;
        min-width: 300px;
        max-width: 80%;
        min-height: auto;
        text-align: left;

        word-wrap: break-word;
        overflow-wrap: break-word;
        white-space: pre-wrap;
        padding: 20px;

        margin-top: 20px;
        margin-bottom: 20px;
        font-size: 1.1rem;
        line-height: 1.6;
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
    }
    .ai-input-message {
        display: flex;
        flex-direction: column;
        background-color: #007bff;
        color: white;
        border-radius: 15px;
        width: auto;
        min-width: 200px;
        max-width: 70%;

        text-align: left;
        word-wrap: break-word;
        overflow-wrap: break-word;
        white-space: pre-wrap;
        padding: 20px;

        margin-top: 20px;
        margin-bottom: 20px;
        margin-left: auto;
        font-size: 1.1rem;
        line-height: 1.6;
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
    }

    .message-header {
        margin-bottom: 10px;
        font-weight: bold;
    }

    .message-content {
        flex: 1;
    }

    /* Styles pour le contenu markdown dans les messages AI */
    .ai-output-message .message-content :global(h1),
    .ai-output-message .message-content :global(h2),
    .ai-output-message .message-content :global(h3),
    .ai-output-message .message-content :global(h4),
    .ai-output-message .message-content :global(h5),
    .ai-output-message .message-content :global(h6) {
        margin-top: 1.5em;
        margin-bottom: 0.5em;
        font-weight: bold;
        color: #333;
    }

    .ai-output-message .message-content :global(h1) { font-size: 1.8em; }
    .ai-output-message .message-content :global(h2) { font-size: 1.5em; }
    .ai-output-message .message-content :global(h3) { font-size: 1.3em; }

    .ai-output-message .message-content :global(p) {
        margin-bottom: 1em;
        line-height: 1.6;
    }

    .ai-output-message .message-content :global(ul),
    .ai-output-message .message-content :global(ol) {
        margin: 1em 0;
        padding-left: 1.5em;
    }

    .ai-output-message .message-content :global(li) {
        margin-bottom: 0.5em;
    }

    .ai-output-message .message-content :global(blockquote) {
        margin: 1em 0;
        padding: 0.5em 1em;
        border-left: 4px solid #007bff;
        background-color: #f8f9fa;
        font-style: italic;
    }

    .ai-output-message .message-content :global(code) {
        background-color: #f1f3f4;
        padding: 2px 4px;
        border-radius: 3px;
        font-family: 'Courier New', monospace;
        font-size: 0.9em;
    }

    .ai-output-message .message-content :global(pre) {
        background-color: #f8f9fa;
        border: 1px solid #e9ecef;
        border-radius: 5px;
        padding: 1em;
        overflow-x: auto;
        margin: 1em 0;
    }

    .ai-output-message .message-content :global(pre code) {
        background-color: transparent;
        padding: 0;
        font-family: 'Courier New', monospace;
        font-size: 0.9em;
    }

    .ai-output-message .message-content :global(a) {
        color: #007bff;
        text-decoration: underline;
    }

    .ai-output-message .message-content :global(a:hover) {
        color: #0056b3;
    }

    .ai-output-message .message-content :global(strong) {
        font-weight: bold;
    }

    .ai-output-message .message-content :global(em) {
        font-style: italic;
    }

    .ai-output-message .message-content :global(table) {
        border-collapse: collapse;
        width: 100%;
        margin: 1em 0;
    }

    .ai-output-message .message-content :global(th),
    .ai-output-message .message-content :global(td) {
        border: 1px solid #ddd;
        padding: 8px;
        text-align: left;
    }

    .ai-output-message .message-content :global(th) {
        background-color: #f2f2f2;
        font-weight: bold;
    }

    
</style>