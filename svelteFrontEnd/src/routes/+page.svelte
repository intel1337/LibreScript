<script>
  import Questions from './Components/Questions.svelte';
  import "../app.css";
  import {onMount} from 'svelte';
  import { triggerPostsRefresh } from '$lib/stores/postStore.js';
  
  onMount(() => {
    triggerPostsRefresh();
  });
</script>

<section>
  <Questions />
</section>
