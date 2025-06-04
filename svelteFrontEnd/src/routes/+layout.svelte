<script>
  import Header from './Components/Header.svelte';
  import { onMount } from 'svelte';
  import { checkAuth } from '$lib/services/authService.js';
  import { page } from '$app/stores';
  import { get } from 'svelte/store';

  onMount(() => {
    const currentPath = get(page).url.pathname;
    // Ne vérifie l'auth que si on n'est PAS sur /login ou /register
    if (currentPath !== '/login' && currentPath !== '/register') {
      checkAuth();
    }
  });
</script>

<div class="layout-container">
    <Header />


  <main>
    <slot /> <!-- This is crucial for page content to render -->
  </main>

  <footer>
    <!-- You can put a global footer here -->
  </footer>
</div>

<style>
 
  /* Optional: Add some basic styling for the layout itself */
  .layout-container {
    display: flex;
    flex-direction: column;
    min-height: 100vh;
    margin: 0;
	padding: 0;


  }

  main {
    flex-grow: 1; /* Ensures main content takes up available space */
    /* Add padding or other styles as needed */
  }

  /* Example: 
  header, footer {
    padding: 1rem;
    background-color: #f0f0f0;
    text-align: center;
  }
  */
</style>
