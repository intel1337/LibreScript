<script>
    import { onMount } from 'svelte';
    
    let visible = false;
    let counter = 0;
    let stop = false;

    onMount(() => {
        visible = true;
    });

    setInterval(() => {
        if (counter > 10000) {
            clearInterval(stop);
            stop = true;
        }
        counter++;
    }, 1);
</script>

<div class="container" class:visible>
    <section class="hero">
        <h1>About LibreScript</h1>
        <p>A place where developers help each other</p>
        <div class="stats">
            <div class="stat">
                <span class="number">{counter}+</span>
                <span>Questions</span>
            </div>
            <div class="stat">
                <span class="number">5K+</span>
                <span>Users</span>
            </div>
        </div>
    </section>

    <section class="features">
        <h2>What we offer</h2>
        <div class="grid">
            <div class="card">
                <h3>Ask Questions</h3>
                <p>Get help from other developers</p>
            </div>
            <div class="card">
                <h3>Share Knowledge</h3>
                <p>Help others with their code</p>
            </div>
        </div>
    </section>

    <section class="tech">
        <h2>Built with</h2>
        <div class="tech-stack">
            <span>SvelteKit</span>
            <span>·</span>
            <span>.NET</span>
            <span>·</span>
            <span>PostgreSQL</span>
        </div>
    </section>

    <footer>
        <p>© 2025 LibreScript</p>
        <a href="mailto:contact@librescript.com">Contact</a>
    </footer>
</div>
<style>
    @import url('https://fonts.googleapis.com/css2?family=Roboto:wght@400;700&display=swap');

    :global(body) {
        font-family: 'Roboto', sans-serif;
    }

    .container {
        max-width: 1200px;
        margin: 0 auto;
        padding: 2rem;
        opacity: 0;
        transform: translateY(20px);
        transition: all 0.6s cubic-bezier(0.4, 0, 0.2, 1);
    }

    .visible {
        opacity: 1;
        transform: translateY(0);
    }

    .hero {
        text-align: center;
        padding: 4rem 0;
        background: linear-gradient(135deg, #f6f8fc 0%, #ffffff 100%);
        border-radius: 1rem;
        box-shadow: 0 4px 6px -1px rgb(0 0 0 / 0.1);
    }

    .stats {
        display: flex;
        justify-content: center;
        gap: 3rem;
        margin: 2rem 0;
    }

    .number {
        font-size: 3rem;
        font-weight: 700;
        background: linear-gradient(135deg, #2563eb 0%, #4f46e5 100%);
        -webkit-background-clip: text;
        -webkit-text-fill-color: transparent;
    }

    .grid {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
        gap: 1.5rem;
        margin: 2rem 0;
    }

    .card {
        padding: 2rem;
        background: white;
        border-radius: 1rem;
        box-shadow: 0 4px 6px -1px rgb(0 0 0 / 0.1);
        transition: transform 0.2s ease;
    }

    .card:hover {
        transform: translateY(-4px);
    }

    .tech-stack {
        text-align: center;
        margin: 3rem 0;
        font-size: 1.1rem;
        color: #4b5563;
    }

    footer {
        text-align: center;
        padding: 2rem 0;
        border-top: 1px solid #e5e7eb;
        margin-top: 3rem;
    }

    footer a {
        color: #2563eb;
        text-decoration: none;
        transition: color 0.2s ease;
    }

    footer a:hover {
        color: #1d4ed8;
    }

    @media (max-width: 640px) {
        .stats {
            flex-direction: column;
            gap: 1.5rem;
        }
        
        .hero {
            padding: 2rem 0;
        }
    }
</style>
