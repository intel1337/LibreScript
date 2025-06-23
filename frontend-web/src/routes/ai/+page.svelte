<script>
  import AiChat from '$lib/components/AiChat.svelte';
  import { currentUser } from '$lib/stores/authStore.js';
  import { onMount } from 'svelte';
  import { goto } from '$app/navigation';

  onMount(() => {
    // Rediriger vers login si pas connecté
    if (!$currentUser) {
      goto('/login');
    }
  });
</script>

<svelte:head>
  <title>IA Assistant - LibreScript</title>
  <meta name="description" content="Assistant IA pour vous aider avec vos questions de programmation" />
</svelte:head>

{#if $currentUser}
  <main class="ai-page">
    <div class="page-header">
      <h1>🤖 Assistant IA LibreScript</h1>
      <p class="page-description">
        Posez vos questions de programmation à notre IA entraînée sur la communauté LibreScript.
        Obtenez des réponses personnalisées et des exemples de code adaptés à vos besoins.
      </p>
    </div>

    <AiChat />

    <div class="ai-info">
      <div class="info-section">
        <h3>💡 Comment bien utiliser l'IA</h3>
        <ul>
          <li><strong>Soyez spécifique :</strong> Plus votre question est précise, meilleure sera la réponse</li>
          <li><strong>Mentionnez le langage :</strong> Spécifiez le langage de programmation pour des réponses ciblées</li>
          <li><strong>Donnez du contexte :</strong> Expliquez ce que vous essayez de faire</li>
          <li><strong>Utilisez les suggestions :</strong> Explorez les exemples de prompts proposés</li>
        </ul>
      </div>

      <div class="info-section">
        <h3>⚙️ Paramètres de génération</h3>
        <ul>
          <li><strong>Longueur :</strong> Contrôle la taille de la réponse (50-500 mots)</li>
          <li><strong>Créativité :</strong> Plus élevée = plus créative, plus basse = plus précise</li>
          <li><strong>Langage :</strong> Aide l'IA à comprendre le contexte technique</li>
          <li><strong>Contexte :</strong> Informations supplémentaires pour améliorer la réponse</li>
        </ul>
      </div>

      <div class="info-section">
        <h3>🚀 Exemples de questions efficaces</h3>
        <div class="examples">
          <div class="example">
            <strong>Python :</strong> "Comment implémenter un décorateur en Python pour mesurer le temps d'exécution d'une fonction ?"
          </div>
          <div class="example">
            <strong>JavaScript :</strong> "Exemple d'utilisation d'async/await pour faire des requêtes API séquentielles"
          </div>
          <div class="example">
            <strong>SQL :</strong> "Requête pour trouver les utilisateurs qui ont posté plus de 5 questions ce mois"
          </div>
        </div>
      </div>
    </div>
  </main>
{:else}
  <div class="auth-required">
    <h2>🔐 Connexion requise</h2>
    <p>Vous devez être connecté pour utiliser l'assistant IA.</p>
    <a href="/login" class="login-link">Se connecter</a>
  </div>
{/if}

<style>
  .ai-page {
    min-height: 100vh;
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    padding: 20px 0;
  }

  .page-header {
    text-align: center;
    color: white;
    margin-bottom: 30px;
    padding: 0 20px;
  }

  .page-header h1 {
    font-size: 2.5rem;
    margin-bottom: 10px;
    text-shadow: 2px 2px 4px rgba(0,0,0,0.3);
  }

  .page-description {
    font-size: 1.1rem;
    max-width: 600px;
    margin: 0 auto;
    opacity: 0.9;
    line-height: 1.6;
  }

  .ai-info {
    max-width: 800px;
    margin: 40px auto 0;
    padding: 0 20px;
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
    gap: 30px;
  }

  .info-section {
    background: rgba(255, 255, 255, 0.95);
    border-radius: 12px;
    padding: 25px;
    box-shadow: 0 8px 32px rgba(0,0,0,0.1);
    backdrop-filter: blur(10px);
  }

  .info-section h3 {
    margin-top: 0;
    color: #333;
    font-size: 1.3rem;
    margin-bottom: 15px;
  }

  .info-section ul {
    list-style: none;
    padding: 0;
    margin: 0;
  }

  .info-section li {
    padding: 8px 0;
    border-bottom: 1px solid #eee;
    line-height: 1.5;
  }

  .info-section li:last-child {
    border-bottom: none;
  }

  .info-section strong {
    color: #007bff;
  }

  .examples {
    display: flex;
    flex-direction: column;
    gap: 15px;
  }

  .example {
    background: #f8f9fa;
    padding: 15px;
    border-radius: 8px;
    border-left: 4px solid #007bff;
    line-height: 1.5;
  }

  .example strong {
    color: #007bff;
    display: block;
    margin-bottom: 5px;
  }

  .auth-required {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    min-height: 60vh;
    text-align: center;
    padding: 40px 20px;
  }

  .auth-required h2 {
    color: #333;
    margin-bottom: 15px;
  }

  .auth-required p {
    color: #666;
    font-size: 1.1rem;
    margin-bottom: 25px;
  }

  .login-link {
    background: #007bff;
    color: white;
    padding: 12px 30px;
    border-radius: 6px;
    text-decoration: none;
    font-weight: 500;
    transition: background-color 0.2s ease;
  }

  .login-link:hover {
    background: #0056b3;
  }

  @media (max-width: 768px) {
    .page-header h1 {
      font-size: 2rem;
    }

    .ai-info {
      grid-template-columns: 1fr;
      gap: 20px;
    }

    .info-section {
      padding: 20px;
    }
  }
</style> 