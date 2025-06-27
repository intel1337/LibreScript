<script>
  import Header from '../Components/Header.svelte';

</script>

<div class="layout-container">




  <main>
    <slot /> <!-- This is crucial for page content to render -->
  </main>


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
