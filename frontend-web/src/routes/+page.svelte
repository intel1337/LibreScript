<script>
  import Questions from './Components/Questions.svelte';
  import "../app.css";
  import {onMount} from 'svelte';
  import { triggerPostsRefresh } from '$lib/stores/postStore.js';
  import NotLogged from './Components/NotLogged.svelte';
  import { isLoggedIn } from '$lib/services/loginService.js';
  onMount(() => {
    triggerPostsRefresh();

  });
</script>

<section>
  {#if !isLoggedIn()}
  <NotLogged /> 
    <Questions />
  {:else}
    <Questions />
  {/if}
</section>


<style>
  section {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
  }
</style>