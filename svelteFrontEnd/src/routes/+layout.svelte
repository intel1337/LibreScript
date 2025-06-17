<script>
  import Header from './Components/Header.svelte';
  import { page } from '$app/stores';
</script>

<div class="layout-container">
    <Header />

  <main>
    <slot /> 
  </main>

  <footer>
  
  </footer>
</div>

<style>
 
  .layout-container {
    display: flex;
    flex-direction: column;
    min-height: 100vh;
    margin: 0;
    padding: 0;
    width: 100%;
    max-width: 100vw;
    overflow-x: hidden;
  }

  main {
    flex-grow: 1; 
    width: 100%;
    padding: 0 1rem;
    box-sizing: border-box;
  }

  @media (max-width: 768px) {
    main {
      padding: 0 0.5rem;
    }
  }

  @media (max-width: 480px) {
    main {
      padding: 0 0.25rem;
    }
  }


</style>
