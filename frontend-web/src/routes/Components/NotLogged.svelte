<script>
    import { onMount } from 'svelte';
    import image from '$lib/images/sad.webp';
</script>

<section>
    <img src={image} alt="logo" style="width: 100px; height: 100px;">
    <a href="/login"><h1>You are not logged in</h1></a>
</section>

<style>
    section {
        margin-top: 1%;
        font-family: 'Inter', 'Roboto', Arial, sans-serif;

        display: flex;
        flex-direction: row;
        align-items: center;
        justify-content: center;
        font-size: 1rem;
        height: 5vh;
        width: 100%;
        color: black;
        gap: 10px;
        animation: bounce 0.7s infinite;
        transition: 0.6s ease-in-out;

     
    }
    @keyframes bounce {
        0% {
            transform: translateY(0);
        }
        50% {
            transform: translateY(-5px);
        }
    }

    img {
        border-radius: 50%;
        max-width: 30px;
        max-height: 30px;
    }
    @media (max-width: 768px) {
        section {
            font-size: 0.85rem;
        }
    }
    a {
        text-decoration: none;
        color: rgb(0, 0, 0);
        transition: 0.6s ease-in-out;
        font-weight: bold;
        transition: 0.6s ease-in-out;

    }
    section:hover {
        translate: 0 10%;
        text-decoration: underline;
        animation: none;
    }
    
</style>