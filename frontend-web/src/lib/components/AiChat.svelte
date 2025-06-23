<script>
  import { onMount } from 'svelte';
  import { currentUser } from '$lib/stores/authStore.js';
  import { 
    generateAiResponse, 
    getAiStatus, 
    checkAiHealth,
    formatPromptForLibreScript,
    validateGenerationOptions,
    getPromptSuggestions 
  } from '$lib/services/aiService.js';

  // État du composant
  let prompt = '';
  let response = '';
  let isLoading = false;
  let error = '';
  let aiStatus = null;
  let isConnected = false;
  let showSettings = false;
  
  // Paramètres de génération
  let length = 200;
  let temperature = 0.7;
  let selectedLanguage = '';
  let context = '';
  
  // Suggestions
  let suggestions = getPromptSuggestions();
  let showSuggestions = false;

  // Vérifier le statut de l'IA au chargement
  onMount(async () => {
    await checkAiConnection();
  });

  async function checkAiConnection() {
    try {
      const health = await checkAiHealth();
      isConnected = health.connected;
      
      if (isConnected) {
        aiStatus = await getAiStatus();
      }
    } catch (err) {
      console.error('Erreur de connexion IA:', err);
      isConnected = false;
    }
  }

  async function handleSubmit() {
    if (!prompt.trim()) {
      error = 'Veuillez entrer un prompt';
      return;
    }

    if (!$currentUser?.token) {
      error = 'Vous devez être connecté pour utiliser l\'IA';
      return;
    }

    // Valider les options
    const validationErrors = validateGenerationOptions({ length, temperature });
    if (validationErrors.length > 0) {
      error = validationErrors.join(', ');
      return;
    }

    isLoading = true;
    error = '';
    response = '';

    try {
      // Formater le prompt
      const formattedPrompt = formatPromptForLibreScript(prompt, selectedLanguage, context);
      
      // Générer la réponse
      const result = await generateAiResponse(
        formattedPrompt, 
        { length, temperature }, 
        $currentUser.token
      );

      response = result.response;
    } catch (err) {
      error = err.message || 'Erreur lors de la génération de réponse';
    } finally {
      isLoading = false;
    }
  }

  function useSuggestion(suggestion) {
    prompt = suggestion.prompt;
    selectedLanguage = suggestion.language;
    showSuggestions = false;
  }

  function clearAll() {
    prompt = '';
    response = '';
    error = '';
    context = '';
    selectedLanguage = '';
  }
</script>

<div class="ai-chat-container">
  <!-- Header avec statut -->
  <div class="ai-header">
    <div class="ai-title">
      <h2>🤖 LibreScript AI Assistant</h2>
      <div class="ai-status">
        <span class="status-indicator {isConnected ? 'connected' : 'disconnected'}"></span>
        <span class="status-text">
          {isConnected ? 'Connecté' : 'Déconnecté'}
        </span>
        {#if aiStatus?.model_loaded}
          <span class="model-info">
            • Modèle chargé ({aiStatus.training_steps || 0} étapes)
          </span>
        {/if}
      </div>
    </div>
    
    <div class="ai-controls">
      <button 
        class="btn-secondary" 
        on:click={() => showSettings = !showSettings}
      >
        ⚙️ Paramètres
      </button>
      <button 
        class="btn-secondary" 
        on:click={() => showSuggestions = !showSuggestions}
      >
        💡 Suggestions
      </button>
      <button 
        class="btn-secondary" 
        on:click={checkAiConnection}
      >
        🔄 Actualiser
      </button>
    </div>
  </div>

  <!-- Message d'erreur de connexion -->
  {#if !isConnected}
    <div class="error-banner">
      ⚠️ L'IA n'est pas disponible. Assurez-vous que l'API Flask est démarrée sur le port 5050.
    </div>
  {/if}

  <!-- Paramètres (repliable) -->
  {#if showSettings}
    <div class="settings-panel">
      <h3>Paramètres de génération</h3>
      <div class="settings-grid">
        <div class="setting-group">
          <label for="language">Langage:</label>
          <select id="language" bind:value={selectedLanguage}>
            <option value="">Aucun</option>
            <option value="python">Python</option>
            <option value="javascript">JavaScript</option>
            <option value="java">Java</option>
            <option value="csharp">C#</option>
            <option value="sql">SQL</option>
            <option value="html">HTML</option>
            <option value="css">CSS</option>
            <option value="other">Autre</option>
          </select>
        </div>
        
        <div class="setting-group">
          <label for="length">Longueur: {length}</label>
          <input 
            type="range" 
            id="length" 
            min="50" 
            max="500" 
            bind:value={length}
          />
        </div>
        
        <div class="setting-group">
          <label for="temperature">Créativité: {temperature}</label>
          <input 
            type="range" 
            id="temperature" 
            min="0.1" 
            max="1.0" 
            step="0.1" 
            bind:value={temperature}
          />
        </div>
      </div>
      
      <div class="setting-group">
        <label for="context">Contexte supplémentaire:</label>
        <textarea 
          id="context" 
          bind:value={context} 
          placeholder="Ajoutez du contexte pour améliorer la réponse..."
          rows="2"
        ></textarea>
      </div>
    </div>
  {/if}

  <!-- Suggestions (repliable) -->
  {#if showSuggestions}
    <div class="suggestions-panel">
      <h3>Suggestions de prompts</h3>
      <div class="suggestions-grid">
        {#each suggestions as suggestion}
          <button 
            class="suggestion-card"
            on:click={() => useSuggestion(suggestion)}
          >
            <div class="suggestion-title">{suggestion.title}</div>
            <div class="suggestion-prompt">{suggestion.prompt}</div>
          </button>
        {/each}
      </div>
    </div>
  {/if}

  <!-- Formulaire principal -->
  <div class="ai-form">
    <div class="prompt-section">
      <label for="prompt">Votre question:</label>
      <textarea 
        id="prompt" 
        bind:value={prompt} 
        placeholder="Posez votre question de programmation ici..."
        rows="4"
        disabled={isLoading || !isConnected}
      ></textarea>
    </div>

    <div class="form-actions">
      <button 
        class="btn-primary" 
        on:click={handleSubmit}
        disabled={isLoading || !isConnected || !prompt.trim()}
      >
        {#if isLoading}
          🔄 Génération en cours...
        {:else}
          🚀 Générer une réponse
        {/if}
      </button>
      
      <button 
        class="btn-secondary" 
        on:click={clearAll}
        disabled={isLoading}
      >
        🗑️ Effacer
      </button>
    </div>
  </div>

  <!-- Erreurs -->
  {#if error}
    <div class="error-message">
      ❌ {error}
    </div>
  {/if}

  <!-- Réponse -->
  {#if response}
    <div class="response-section">
      <h3>🤖 Réponse de l'IA:</h3>
      <div class="response-content">
        <pre>{response}</pre>
      </div>
      <div class="response-actions">
        <button 
          class="btn-secondary"
          on:click={() => navigator.clipboard.writeText(response)}
        >
          📋 Copier
        </button>
      </div>
    </div>
  {/if}
</div>

<style>
  .ai-chat-container {
    max-width: 800px;
    margin: 0 auto;
    padding: 20px;
    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  }

  .ai-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 20px;
    padding-bottom: 15px;
    border-bottom: 2px solid #e0e0e0;
  }

  .ai-title h2 {
    margin: 0;
    color: #333;
  }

  .ai-status {
    display: flex;
    align-items: center;
    gap: 8px;
    font-size: 14px;
    color: #666;
    margin-top: 5px;
  }

  .status-indicator {
    width: 10px;
    height: 10px;
    border-radius: 50%;
  }

  .status-indicator.connected {
    background-color: #4CAF50;
  }

  .status-indicator.disconnected {
    background-color: #f44336;
  }

  .ai-controls {
    display: flex;
    gap: 10px;
  }

  .error-banner {
    background-color: #fff3cd;
    border: 1px solid #ffeaa7;
    color: #856404;
    padding: 12px;
    border-radius: 8px;
    margin-bottom: 20px;
  }

  .settings-panel, .suggestions-panel {
    background-color: #f8f9fa;
    border: 1px solid #e0e0e0;
    border-radius: 8px;
    padding: 20px;
    margin-bottom: 20px;
  }

  .settings-panel h3, .suggestions-panel h3 {
    margin-top: 0;
    color: #333;
  }

  .settings-grid {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
    gap: 15px;
    margin-bottom: 15px;
  }

  .setting-group {
    display: flex;
    flex-direction: column;
    gap: 5px;
  }

  .setting-group label {
    font-weight: 500;
    color: #555;
  }

  .setting-group input, .setting-group select, .setting-group textarea {
    padding: 8px;
    border: 1px solid #ddd;
    border-radius: 4px;
    font-size: 14px;
  }

  .suggestions-grid {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
    gap: 10px;
  }

  .suggestion-card {
    background: white;
    border: 1px solid #ddd;
    border-radius: 8px;
    padding: 12px;
    text-align: left;
    cursor: pointer;
    transition: all 0.2s ease;
  }

  .suggestion-card:hover {
    border-color: #007bff;
    transform: translateY(-2px);
    box-shadow: 0 4px 8px rgba(0,0,0,0.1);
  }

  .suggestion-title {
    font-weight: 600;
    color: #333;
    margin-bottom: 5px;
  }

  .suggestion-prompt {
    font-size: 13px;
    color: #666;
    line-height: 1.4;
  }

  .ai-form {
    background: white;
    border: 1px solid #e0e0e0;
    border-radius: 8px;
    padding: 20px;
    margin-bottom: 20px;
  }

  .prompt-section label {
    display: block;
    font-weight: 500;
    color: #333;
    margin-bottom: 8px;
  }

  .prompt-section textarea {
    width: 100%;
    padding: 12px;
    border: 1px solid #ddd;
    border-radius: 6px;
    font-size: 14px;
    resize: vertical;
    font-family: inherit;
  }

  .form-actions {
    display: flex;
    gap: 10px;
    margin-top: 15px;
  }

  .btn-primary, .btn-secondary {
    padding: 10px 20px;
    border-radius: 6px;
    font-size: 14px;
    font-weight: 500;
    cursor: pointer;
    transition: all 0.2s ease;
    border: none;
  }

  .btn-primary {
    background-color: #007bff;
    color: white;
  }

  .btn-primary:hover:not(:disabled) {
    background-color: #0056b3;
  }

  .btn-primary:disabled {
    background-color: #ccc;
    cursor: not-allowed;
  }

  .btn-secondary {
    background-color: #6c757d;
    color: white;
  }

  .btn-secondary:hover:not(:disabled) {
    background-color: #545b62;
  }

  .error-message {
    background-color: #f8d7da;
    border: 1px solid #f5c6cb;
    color: #721c24;
    padding: 12px;
    border-radius: 6px;
    margin-bottom: 20px;
  }

  .response-section {
    background: white;
    border: 1px solid #e0e0e0;
    border-radius: 8px;
    padding: 20px;
  }

  .response-section h3 {
    margin-top: 0;
    color: #333;
  }

  .response-content {
    background-color: #f8f9fa;
    border: 1px solid #e0e0e0;
    border-radius: 6px;
    padding: 15px;
    margin-bottom: 15px;
  }

  .response-content pre {
    margin: 0;
    white-space: pre-wrap;
    word-wrap: break-word;
    font-family: 'Courier New', monospace;
    font-size: 14px;
    line-height: 1.5;
  }

  .response-actions {
    display: flex;
    gap: 10px;
  }

  @media (max-width: 768px) {
    .ai-header {
      flex-direction: column;
      align-items: flex-start;
      gap: 15px;
    }

    .ai-controls {
      flex-wrap: wrap;
    }

    .settings-grid {
      grid-template-columns: 1fr;
    }

    .suggestions-grid {
      grid-template-columns: 1fr;
    }

    .form-actions {
      flex-direction: column;
    }
  }
</style> 