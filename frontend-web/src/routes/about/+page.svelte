<script>
    import { onMount } from 'svelte';
    
    let visible = false;
    
    onMount(() => {
        visible = true;
    });
    let counter = 0;
    let stop = false;
    setInterval(() => {
        if (counter > 10000) {
            clearInterval(stop);
            stop = true;
        }
        counter ++;
    }, 1);



</script>

<div class="about-container" class:visible>
    <!-- Hero Section -->
    <section class="hero-section">
        <div class="hero-content">
            <h1 class="hero-title">
                <span class="emoji">📘</span>
                About LibreScript
            </h1>
            <p class="hero-subtitle">
                Empowering developers through collaborative knowledge sharing
            </p>
            <div class="hero-stats">
                <div class="stat">
                    <span class="stat-number">{counter}+</span>
                    <span class="stat-label">Questions</span>
                </div>
                <div class="stat">
                    <span class="stat-number">5K+</span>
                    <span class="stat-label">Developers</span>
                </div>
                <div class="stat">
                    <span class="stat-number">50K+</span>
                    <span class="stat-label">Answers</span>
                </div>
            </div>
        </div>
    </section>

    <!-- Mission Section -->
    <section class="cards-section">
        <div class="section-header">
            <h2>Our Mission</h2>
            <p>Building the future of developer collaboration</p>
        </div>
        
        <div class="cards-grid">
            <div class="card mission-card">
                <div class="card-icon">🎯</div>
                <h3>Community-Driven</h3>
                <p>A platform built by developers, for developers. We believe in the power of collective knowledge and collaborative problem-solving.</p>
            </div>
            
            <div class="card mission-card">
                <div class="card-icon">🚀</div>
                <h3>Knowledge Sharing</h3>
                <p>Empowering knowledge sharing in programming, software engineering, and computer science through structured discussions.</p>
            </div>
            
            <div class="card mission-card">
                <div class="card-icon">🌟</div>
                <h3>Inclusive Environment</h3>
                <p>Whether you're a beginner or expert, LibreScript offers a supportive, inclusive, and constructive environment for all.</p>
            </div>
        </div>
    </section>

    <!-- Features Section -->
    <section class="cards-section">
        <div class="section-header">
            <h2>What You Can Do</h2>
            <p>Discover all the ways to engage with our community</p>
        </div>
        
        <div class="cards-grid">
            <div class="card feature-card">
                <div class="card-icon">❓</div>
                <h3>Ask Questions</h3>
                <p>Post technical questions and get help from experienced developers worldwide.</p>
            </div>
            
            <div class="card feature-card">
                <div class="card-icon">💡</div>
                <h3>Share Solutions</h3>
                <p>Contribute your knowledge by providing detailed answers and solutions.</p>
            </div>
            
            <div class="card feature-card">
                <div class="card-icon">👍</div>
                <h3>Vote & Curate</h3>
                <p>Help the community by voting for the most helpful and accurate contributions.</p>
            </div>
            
            <div class="card feature-card">
                <div class="card-icon">📚</div>
                <h3>Build Knowledge</h3>
                <p>Contribute to a collective knowledge base that's accessible to everyone.</p>
            </div>
        </div>
    </section>

    <!-- Technology Stack -->
    <section class="cards-section">
        <div class="section-header">
            <h2>Built With Modern Tech</h2>
            <p>Powered by cutting-edge technologies</p>
        </div>
        
        <div class="tech-grid">
            <div class="tech-card">
                <div class="tech-icon">⚡</div>
                <h4>SvelteKit</h4>
                <p>Frontend</p>
            </div>
            <div class="tech-card">
                <div class="tech-icon">🔷</div>
                <h4>.NET Core</h4>
                <p>Backend</p>
            </div>
            <div class="tech-card">
                <div class="tech-icon">🐘</div>
                <h4>PostgreSQL</h4>
                <p>Database</p>
            </div>
            <div class="tech-card">
                <div class="tech-icon">🔐</div>
                <h4>JWT Auth</h4>
                <p>Security</p>
            </div>
        </div>
    </section>

    <!-- GDPR Compliance -->
    <section class="cards-section">
        <div class="section-header">
            <h2>🛡️ Privacy & Compliance</h2>
            <p>Your data protection is our priority</p>
        </div>
        
        <div class="cards-grid">
            <div class="card privacy-card">
                <div class="card-icon">📋</div>
                <h3>Data Collection</h3>
                <ul>
                    <li>Username and email (registration)</li>
                    <li>Content you submit (questions, answers)</li>
                    <li>Usage analytics (anonymous)</li>
                    <li>Security logs (IP addresses)</li>
                </ul>
            </div>
            
            <div class="card privacy-card">
                <div class="card-icon">⚖️</div>
                <h3>Your Rights</h3>
                <ul>
                    <li>Access your personal data</li>
                    <li>Correct or delete your data</li>
                    <li>Withdraw consent anytime</li>
                    <li>Data portability</li>
                    <li>File complaints with authorities</li>
                </ul>
            </div>
            
            <div class="card privacy-card">
                <div class="card-icon">🎯</div>
                <h3>Data Purpose</h3>
                <ul>
                    <li>Account management</li>
                    <li>Content display</li>
                    <li>Community moderation</li>
                    <li>Service improvement</li>
                    <li>Legal compliance</li>
                </ul>
            </div>
        </div>
    </section>

    <!-- Contact Section -->
    <section class="contact-section">
        <div class="contact-card">
            <h2>Get in Touch</h2>
            <p>Have questions about LibreScript or need help with your account?</p>
            <div class="contact-info">
                <div class="contact-item">
                    <span class="contact-icon">📧</span>
                    <span>contact@librescript.com</span>
                </div>
                <div class="contact-item">
                    <span class="contact-icon">🛡️</span>
                    <span>privacy@librescript.com</span>
                </div>
            </div>
            <div class="social-links">
                <a href="#" class="social-link">GitHub</a>
                <a href="#" class="social-link">Twitter</a>
                <a href="#" class="social-link">Discord</a>
            </div>
        </div>
    </section>

    <!-- Footer -->
    <footer class="about-footer">
        <p>&copy; 2025 LibreScript. All rights reserved.</p>
        <p>LibreScript is committed to GDPR compliance and data protection.</p>
    </footer>
</div>

<style>
    @import url('https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700&display=swap');
    
    .about-container {
        font-family: 'Inter', sans-serif;
        max-width: 1200px;
        margin: 0 auto;
        padding: 0 20px;
        opacity: 0;
        transform: translateY(20px);
        transition: all 0.8s ease;
    }
    
    .about-container.visible {
        opacity: 1;
        transform: translateY(0);
    }

    /* Hero Section */
    .hero-section {
        margin-top: 5%;
        text-align: center;
        padding: 80px 0;
        background-color: #166fff;
        border-radius: 20px;
        margin-bottom: 60px;
        color: white;
        position: relative;
        overflow: hidden;
        

    }
    
    .hero-section::before {
        content: '';
        position: absolute;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background: url('data:image/svg+xml,<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 100"><defs><pattern id="grain" width="100" height="100" patternUnits="userSpaceOnUse"><circle cx="25" cy="25" r="1" fill="white" opacity="0.1"/><circle cx="75" cy="75" r="1" fill="white" opacity="0.1"/><circle cx="50" cy="10" r="1" fill="white" opacity="0.1"/></pattern></defs><rect width="100" height="100" fill="url(%23grain)"/></svg>');
        opacity: 0.3;
    }
    
    .hero-content {
        position: static;
        z-index: 1;
    }
    
    .hero-title {
        font-size: 3.5rem;
        font-weight: 700;
        margin-bottom: 20px;
        line-height: 1.2;
    }
    
    .emoji {
        display: inline-block;
        margin-right: 15px;
        font-size: 4rem;
    }
    
    .hero-subtitle {
        font-size: 1.3rem;
        margin-bottom: 40px;
        opacity: 0.9;
        font-weight: 300;
    }
    
    .hero-stats {
        display: flex;
        justify-content: center;
        gap: 60px;
        margin-top: 40px;
    }
    
    .stat {
        text-align: center;
    }
    
    .stat-number {
        display: block;
        font-size: 2.5rem;
        font-weight: 700;
        margin-bottom: 5px;
    }
    
    .stat-label {
        font-size: 1rem;
        opacity: 0.8;
        font-weight: 400;
    }

    /* Sections */
    .cards-section {
        margin-bottom: 80px;
    }
    
    .section-header {
        text-align: center;
        margin-bottom: 50px;
    }
    
    .section-header h2 {
        font-size: 2.5rem;
        font-weight: 600;
        color: #2d3748;
        margin-bottom: 15px;
    }
    
    .section-header p {
        font-size: 1.2rem;
        color: #718096;
        font-weight: 400;
    }

    /* Cards Grid */
    .cards-grid {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
        gap: 30px;
        margin-bottom: 40px;
    }
    
    .card {
        background: white;
        border-radius: 16px;
        padding: 40px 30px;
        box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
        transition: all 0.3s ease;
        border: 1px solid #e2e8f0;
        text-align: center;
        position: relative;
        overflow: hidden;
    }
    
    .card::before {
        content: '';
        position: absolute;
        top: 0;
        left: 0;
        right: 0;
        height: 4px;
        background-color: #166fff;
        transform: scaleX(0);
        transition: transform 0.3s ease;
    }
    
    .card:hover {
        transform: translateY(-8px);

    }
    
    .card:hover::before {
        transform: scaleX(1);
    }
    
    .card-icon {
        font-size: 3rem;
        margin-bottom: 20px;
        display: block;
    }
    
    .card h3 {
        font-size: 1.5rem;
        font-weight: 600;
        color: #2d3748;
        margin-bottom: 15px;
    }
    
    .card p {
        color: #718096;
        line-height: 1.6;
        font-size: 1rem;
    }
    
    .card ul {
        text-align: left;
        color: #718096;
        line-height: 1.8;
    }
    
    .card ul li {
        margin-bottom: 8px;
    }

    /* Tech Grid */
    .tech-grid {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
        gap: 20px;
        margin-bottom: 40px;
    }
    
    .tech-card {
        background: white;
        border-radius: 12px;
        padding: 30px 20px;
        text-align: center;
        box-shadow: 0 2px 12px rgba(0, 0, 0, 0.05);
        transition: all 0.3s ease;
        border: 1px solid #e2e8f0;
    }
    
    .tech-card:hover {
        transform: translateY(-4px);
        box-shadow: 0 8px 25px rgba(0, 0, 0, 0.1);
    }
    
    .tech-icon {
        font-size: 2.5rem;
        margin-bottom: 15px;
    }
    
    .tech-card h4 {
        font-size: 1.2rem;
        font-weight: 600;
        color: #2d3748;
        margin-bottom: 5px;
    }
    
    .tech-card p {
        color: #718096;
        font-size: 0.9rem;
    }

    /* Contact Section */
    .contact-section {
        margin-bottom: 60px;
    }
    
    .contact-card {
        background-color: #166fff;
        color: white;
        border-radius: 20px;
        padding: 60px 40px;
        text-align: center;
    }
    
    .contact-card h2 {
        font-size: 2.5rem;
        font-weight: 600;
        margin-bottom: 15px;
    }
    
    .contact-card > p {
        font-size: 1.2rem;
        margin-bottom: 40px;
        opacity: 0.9;
    }
    
    .contact-info {
        display: flex;
        justify-content: center;
        gap: 40px;
        margin-bottom: 30px;
        flex-wrap: wrap;
    }
    
    .contact-item {
        display: flex;
        align-items: center;
        gap: 10px;
        font-size: 1.1rem;
    }
    
    .contact-icon {
        font-size: 1.3rem;
    }
    
    .social-links {
        display: flex;
        justify-content: center;
        gap: 20px;
        margin-top: 30px;
    }
    
    .social-link {
        background: rgba(255, 255, 255, 0.2);
        color: white;
        padding: 12px 24px;
        border-radius: 25px;
        text-decoration: none;
        font-weight: 500;
        transition: all 0.3s ease;
        border: 1px solid rgba(255, 255, 255, 0.3);
    }
    
    .social-link:hover {
        background: rgba(255, 255, 255, 0.3);
        transform: translateY(-2px);
    }

    /* Footer */
    .about-footer {
        text-align: center;
        padding: 40px 0;
        border-top: 1px solid #e2e8f0;
        color: #718096;
    }
    
    .about-footer p {
        margin-bottom: 10px;
    }

    /* Responsive Design */
    @media (max-width: 768px) {
        .hero-title {
            font-size: 2.5rem;
        }
        
        .hero-stats {
            gap: 30px;
            flex-wrap: wrap;
        }
        
        .stat-number {
            font-size: 2rem;
        }
        
        .section-header h2 {
            font-size: 2rem;
        }
        
        .cards-grid {
            grid-template-columns: 1fr;
        }
        
        .tech-grid {
            grid-template-columns: repeat(2, 1fr);
        }
        
        .contact-info {
            flex-direction: column;
            gap: 20px;
        }
        
        .social-links {
            flex-wrap: wrap;
        }
    }
    
    @media (max-width: 480px) {
        .about-container {
            padding: 0 15px;
        }
        
        .hero-section {
            padding: 60px 20px;
        }
        
        .hero-title {
            font-size: 2rem;
        }
        
        .emoji {
            font-size: 2.5rem;
        }
        
        .card {
            padding: 30px 20px;
        }
        
        .contact-card {
            padding: 40px 20px;
        }
        
        .tech-grid {
            grid-template-columns: 1fr;
        }
    }
</style>