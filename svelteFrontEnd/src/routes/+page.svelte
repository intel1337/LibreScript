<script>
  import Questions from './Components/Questions.svelte';
  import "../app.css";
  import {onMount} from 'svelte';
  import {getTags} from '$lib/services/tagService.js';
    import { checkAuth } from '$lib/services/authService';
    

</script>


<section>
  <Questions />
</section>
