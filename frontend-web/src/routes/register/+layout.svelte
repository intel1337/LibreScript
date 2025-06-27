<script>
  import Header from '../Components/Header.svelte';
  // SUPPRESSION de checkAuth sur la page register
</script>

<div class="layout-container">
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
  }

  main {
    flex-grow: 1; 
  }


</style>
