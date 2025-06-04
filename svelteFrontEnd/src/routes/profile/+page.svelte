<script>
 import { checkAuth } from '$lib/services/authService.js';

  onMount(() => {
    checkAuth(); // Vérifie le JWT à chaque chargement de layout
  });
</script>

<section class="profile-page">
    <h1>Profil utilisateur</h1>
    <p>Bienvenue sur votre page de profil !</p>
    <!-- Ajoute ici les infos du user, avatar, etc. -->
</section>

<style>
.profile-page {
    max-width: 600px;
    margin: 2rem auto;
    background: #fff;
    border-radius: 12px;
    box-shadow: 0 2px 16px rgba(0,0,0,0.07);
    padding: 2rem;
    text-align: center;
}
.profile-page h1 {
    font-size: 2rem;
    margin-bottom: 1rem;
}
</style>
