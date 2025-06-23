<script>
    import { onMount } from 'svelte';
    
    let visible = false;
    
    onMount(() => {
        visible = true;
        // Scroll to top
        window.scrollTo(0, 0);
    });
</script>

<svelte:head>
    <title>Terms of Service - LibreScript</title>
    <meta name="description" content="Terms of Service and Privacy Policy for LibreScript - Developer Q&A Platform" />
</svelte:head>

<div class="tos-container" class:visible>
    <!-- Header -->
    <section class="tos-header">
        <div class="header-content">
            <h1>📋 Terms of Service</h1>
            <p class="subtitle">Legal Terms and Privacy Policy</p>
            <div class="last-updated">
                <span>Last Updated: January 1, 2025</span>
            </div>
        </div>
    </section>

    <!-- Navigation -->
    <nav class="tos-nav">
        <div class="nav-content">
            <a href="#terms" class="nav-link">Terms of Service</a>
            <a href="#privacy" class="nav-link">Privacy Policy</a>
            <a href="#cookies" class="nav-link">Cookie Policy</a>
            <a href="#gdpr" class="nav-link">GDPR Rights</a>
            <a href="#contact" class="nav-link">Contact</a>
        </div>
    </nav>

    <!-- Terms of Service Section -->
    <section id="terms" class="content-section">
        <div class="section-card">
            <h2>📜 Terms of Service</h2>
            
            <div class="subsection">
                <h3>1. Acceptance of Terms</h3>
                <p>By accessing and using LibreScript ("the Service"), you accept and agree to be bound by the terms and provision of this agreement. If you do not agree to abide by the above, please do not use this service.</p>
            </div>

            <div class="subsection">
                <h3>2. Description of Service</h3>
                <p>LibreScript is a community-driven question-and-answer platform for developers, programmers, and technology enthusiasts. The Service allows users to:</p>
                <ul>
                    <li>Ask and answer technical questions</li>
                    <li>Share knowledge and solutions</li>
                    <li>Vote on content quality</li>
                    <li>Build a professional profile</li>
                    <li>Participate in community discussions</li>
                </ul>
            </div>

            <div class="subsection">
                <h3>3. User Accounts and Registration</h3>
                <p>To access certain features of the Service, you must register for an account. You agree to:</p>
                <ul>
                    <li>Provide accurate, current, and complete information</li>
                    <li>Maintain and update your information to keep it accurate</li>
                    <li>Maintain the security of your password</li>
                    <li>Accept responsibility for all activities under your account</li>
                    <li>Notify us immediately of any unauthorized use</li>
                </ul>
            </div>

            <div class="subsection">
                <h3>4. User Conduct and Content</h3>
                <p>You agree not to use the Service to:</p>
                <ul>
                    <li>Post spam, duplicate content, or irrelevant material</li>
                    <li>Harass, abuse, or harm other users</li>
                    <li>Share personal information of others without consent</li>
                    <li>Post malicious code, viruses, or harmful content</li>
                    <li>Violate any applicable laws or regulations</li>
                    <li>Impersonate others or provide false information</li>
                    <li>Attempt to gain unauthorized access to the system</li>
                </ul>
            </div>

            <div class="subsection">
                <h3>5. Content Ownership and License</h3>
                <p>You retain ownership of content you post. By posting content, you grant LibreScript a worldwide, non-exclusive, royalty-free license to use, display, and distribute your content on the platform.</p>
                <p>You represent that you have the right to grant this license and that your content does not infringe on others' rights.</p>
            </div>

            <div class="subsection">
                <h3>6. Moderation and Removal</h3>
                <p>LibreScript reserves the right to:</p>
                <ul>
                    <li>Remove content that violates these terms</li>
                    <li>Suspend or terminate accounts for violations</li>
                    <li>Modify or discontinue the Service</li>
                    <li>Update these terms at any time</li>
                </ul>
            </div>

            <div class="subsection">
                <h3>7. Disclaimer of Warranties</h3>
                <p>The Service is provided "as is" without warranties of any kind. LibreScript does not guarantee the accuracy, completeness, or reliability of any content or information on the platform.</p>
            </div>

            <div class="subsection">
                <h3>8. Limitation of Liability</h3>
                <p>LibreScript shall not be liable for any indirect, incidental, special, consequential, or punitive damages arising from your use of the Service.</p>
            </div>
        </div>
    </section>

    <!-- Privacy Policy Section -->
    <section id="privacy" class="content-section">
        <div class="section-card">
            <h2>🔒 Privacy Policy</h2>
            
            <div class="subsection">
                <h3>1. Information We Collect</h3>
                <h4>Personal Information:</h4>
                <ul>
                    <li><strong>Account Information:</strong> Username, email address, full name</li>
                    <li><strong>Profile Information:</strong> Bio, profile picture, reputation score</li>
                    <li><strong>Content:</strong> Questions, answers, comments, votes</li>
                </ul>
                
                <h4>Technical Information:</h4>
                <ul>
                    <li><strong>Usage Data:</strong> Pages visited, time spent, click patterns</li>
                    <li><strong>Device Information:</strong> IP address, browser type, operating system</li>
                    <li><strong>Cookies:</strong> Session data, preferences, analytics</li>
                </ul>
            </div>

            <div class="subsection">
                <h3>2. How We Use Your Information</h3>
                <p>We use your information to:</p>
                <ul>
                    <li>Provide and improve the Service</li>
                    <li>Authenticate users and prevent fraud</li>
                    <li>Personalize user experience</li>
                    <li>Send important notifications</li>
                    <li>Analyze usage patterns and improve features</li>
                    <li>Comply with legal obligations</li>
                </ul>
            </div>

            <div class="subsection">
                <h3>3. Information Sharing</h3>
                <p>We do not sell your personal information. We may share information:</p>
                <ul>
                    <li><strong>Publicly:</strong> Your username, profile, and content are visible to other users</li>
                    <li><strong>Service Providers:</strong> Trusted third parties who assist in operating the Service</li>
                    <li><strong>Legal Requirements:</strong> When required by law or to protect rights and safety</li>
                    <li><strong>Business Transfers:</strong> In case of merger, acquisition, or sale of assets</li>
                </ul>
            </div>

            <div class="subsection">
                <h3>4. Data Security</h3>
                <p>We implement appropriate security measures to protect your information:</p>
                <ul>
                    <li>Encryption of data in transit and at rest</li>
                    <li>Regular security audits and updates</li>
                    <li>Access controls and authentication</li>
                    <li>Secure hosting infrastructure</li>
                </ul>
            </div>

            <div class="subsection">
                <h3>5. Data Retention</h3>
                <p>We retain your information for as long as necessary to provide the Service and comply with legal obligations. You may request deletion of your account and associated data at any time.</p>
            </div>
        </div>
    </section>

    <!-- Cookie Policy Section -->
    <section id="cookies" class="content-section">
        <div class="section-card">
            <h2>🍪 Cookie Policy</h2>
            
            <div class="subsection">
                <h3>What Are Cookies</h3>
                <p>Cookies are small text files stored on your device when you visit our website. They help us provide a better user experience.</p>
            </div>

            <div class="subsection">
                <h3>Types of Cookies We Use</h3>
                <div class="cookie-types">
                    <div class="cookie-type">
                        <h4>🔧 Essential Cookies</h4>
                        <p>Required for the website to function properly. These include authentication and security cookies.</p>
                    </div>
                    <div class="cookie-type">
                        <h4>📊 Analytics Cookies</h4>
                        <p>Help us understand how users interact with the website to improve our services.</p>
                    </div>
                    <div class="cookie-type">
                        <h4>⚙️ Functional Cookies</h4>
                        <p>Remember your preferences and settings to enhance your experience.</p>
                    </div>
                </div>
            </div>

            <div class="subsection">
                <h3>Managing Cookies</h3>
                <p>You can control cookies through your browser settings. Note that disabling essential cookies may affect website functionality.</p>
            </div>
        </div>
    </section>

    <!-- GDPR Rights Section -->
    <section id="gdpr" class="content-section">
        <div class="section-card">
            <h2>🛡️ Your GDPR Rights</h2>
            
            <div class="subsection">
                <h3>Your Rights Under GDPR</h3>
                <p>If you are in the European Union, you have the following rights:</p>
                
                <div class="rights-grid">
                    <div class="right-card">
                        <h4>📋 Right to Access</h4>
                        <p>Request a copy of your personal data we hold</p>
                    </div>
                    <div class="right-card">
                        <h4>✏️ Right to Rectification</h4>
                        <p>Correct inaccurate or incomplete data</p>
                    </div>
                    <div class="right-card">
                        <h4>🗑️ Right to Erasure</h4>
                        <p>Request deletion of your personal data</p>
                    </div>
                    <div class="right-card">
                        <h4>⏸️ Right to Restrict Processing</h4>
                        <p>Limit how we use your data</p>
                    </div>
                    <div class="right-card">
                        <h4>📦 Right to Data Portability</h4>
                        <p>Receive your data in a portable format</p>
                    </div>
                    <div class="right-card">
                        <h4>⛔ Right to Object</h4>
                        <p>Object to processing of your data</p>
                    </div>
                </div>
            </div>

            <div class="subsection">
                <h3>Legal Basis for Processing</h3>
                <p>We process your data based on:</p>
                <ul>
                    <li><strong>Consent:</strong> When you agree to our terms and privacy policy</li>
                    <li><strong>Contract:</strong> To provide the Service you requested</li>
                    <li><strong>Legitimate Interest:</strong> To improve and secure our Service</li>
                    <li><strong>Legal Obligation:</strong> To comply with applicable laws</li>
                </ul>
            </div>

            <div class="subsection">
                <h3>Data Protection Officer</h3>
                <p>For GDPR-related inquiries, contact our Data Protection Officer:</p>
                <div class="contact-info">
                    <p><strong>Email:</strong> dpo@librescript.com</p>
                    <p><strong>Response Time:</strong> Within 30 days</p>
                </div>
            </div>
        </div>
    </section>

    <!-- Contact Section -->
    <section id="contact" class="content-section">
        <div class="section-card">
            <h2>📞 Contact Information</h2>
            
            <div class="contact-grid">
                <div class="contact-card">
                    <h3>📧 General Inquiries</h3>
                    <p>contact@librescript.com</p>
                </div>
                <div class="contact-card">
                    <h3>🛡️ Privacy & GDPR</h3>
                    <p>privacy@librescript.com</p>
                </div>
                <div class="contact-card">
                    <h3>⚖️ Legal Issues</h3>
                    <p>legal@librescript.com</p>
                </div>
                <div class="contact-card">
                    <h3>🚨 Report Abuse</h3>
                    <p>abuse@librescript.com</p>
                </div>
            </div>

            <div class="subsection">
                <h3>Company Information</h3>
                <div class="company-info">
                    <p><strong>LibreScript Inc.</strong></p>
                    <p>123 Developer Street</p>
                    <p>Tech City, TC 12345</p>
                    <p>United States</p>
                </div>
            </div>
        </div>
    </section>

    <!-- Footer -->
    <footer class="tos-footer">
        <div class="footer-content">
            <p>&copy; 2025 LibreScript Inc. All rights reserved.</p>
            <p>By using LibreScript, you agree to these Terms of Service and Privacy Policy.</p>
            <div class="footer-links">
                <a href="/">Home</a>
                <a href="/about">About</a>
                <a href="/contact">Contact</a>
            </div>
        </div>
    </footer>
</div>

<style>
    @import url('https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700&display=swap');
    
    .tos-container {
        font-family: 'Inter', sans-serif;
        line-height: 1.6;
        color: #374151;
        opacity: 0;
        transform: translateY(20px);
        transition: all 0.8s ease;
    }
    
    .tos-container.visible {
        opacity: 1;
        transform: translateY(0);
    }

    /* Header */
    .tos-header {
        background: linear-gradient(135deg, #166fff 0%, #0d47a1 100%);
        color: white;
        padding: 60px 20px;
        text-align: center;
    }
    
    .header-content {
        max-width: 800px;
        margin: 0 auto;
    }
    
    .tos-header h1 {
        font-size: 3rem;
        font-weight: 700;
        margin-bottom: 15px;
    }
    
    .subtitle {
        font-size: 1.3rem;
        margin-bottom: 20px;
        opacity: 0.9;
    }
    
    .last-updated {
        background: rgba(255, 255, 255, 0.2);
        padding: 8px 16px;
        border-radius: 20px;
        display: inline-block;
        font-size: 0.9rem;
    }

    /* Navigation */
    .tos-nav {
        background: white;
        border-bottom: 1px solid #e5e7eb;
        position: sticky;
        top: 0;
        z-index: 100;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    }
    
    .nav-content {
        max-width: 1200px;
        margin: 0 auto;
        display: flex;
        justify-content: center;
        gap: 30px;
        padding: 15px 20px;
        flex-wrap: wrap;
    }
    
    .nav-link {
        color: #6b7280;
        text-decoration: none;
        font-weight: 500;
        padding: 8px 16px;
        border-radius: 6px;
        transition: all 0.2s ease;
    }
    
    .nav-link:hover {
        color: #166fff;
        background: #f3f4f6;
    }

    /* Content Sections */
    .content-section {
        max-width: 1000px;
        margin: 0 auto;
        padding: 40px 20px;
    }
    
    .section-card {
        background: white;
        border-radius: 12px;
        padding: 40px;
        box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
        border: 1px solid #e5e7eb;
    }
    
    .section-card h2 {
        font-size: 2.5rem;
        font-weight: 600;
        color: #1f2937;
        margin-bottom: 30px;
        border-bottom: 3px solid #166fff;
        padding-bottom: 15px;
    }
    
    .subsection {
        margin-bottom: 40px;
    }
    
    .subsection h3 {
        font-size: 1.5rem;
        font-weight: 600;
        color: #374151;
        margin-bottom: 15px;
    }
    
    .subsection h4 {
        font-size: 1.2rem;
        font-weight: 600;
        color: #4b5563;
        margin: 20px 0 10px 0;
    }
    
    .subsection p {
        margin-bottom: 15px;
        font-size: 1rem;
    }
    
    .subsection ul {
        margin: 15px 0;
        padding-left: 20px;
    }
    
    .subsection li {
        margin-bottom: 8px;
    }

    /* Cookie Types */
    .cookie-types {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
        gap: 20px;
        margin: 20px 0;
    }
    
    .cookie-type {
        background: #f8fafc;
        padding: 20px;
        border-radius: 8px;
        border-left: 4px solid #166fff;
    }
    
    .cookie-type h4 {
        margin-top: 0;
        color: #166fff;
    }

    /* Rights Grid */
    .rights-grid {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
        gap: 20px;
        margin: 30px 0;
    }
    
    .right-card {
        background: #f8fafc;
        padding: 20px;
        border-radius: 12px;
        border: 1px solid #e5e7eb;
        transition: all 0.3s ease;
    }
    
    .right-card:hover {
        transform: translateY(-2px);
        box-shadow: 0 8px 25px rgba(0, 0, 0, 0.1);
    }
    
    .right-card h4 {
        color: #166fff;
        margin-top: 0;
        margin-bottom: 10px;
    }

    /* Contact Grids */
    .contact-grid {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
        gap: 20px;
        margin: 30px 0;
    }
    
    .contact-card {
        background: #f8fafc;
        padding: 25px;
        border-radius: 12px;
        text-align: center;
        border: 1px solid #e5e7eb;
    }
    
    .contact-card h3 {
        color: #166fff;
        margin-bottom: 10px;
    }
    
    .contact-info,
    .company-info {
        background: #f8fafc;
        padding: 20px;
        border-radius: 8px;
        margin: 20px 0;
    }

    /* Footer */
    .tos-footer {
        background: #1f2937;
        color: white;
        padding: 40px 20px;
        text-align: center;
    }
    
    .footer-content {
        max-width: 800px;
        margin: 0 auto;
    }
    
    .footer-links {
        margin-top: 20px;
        display: flex;
        justify-content: center;
        gap: 20px;
        flex-wrap: wrap;
    }
    
    .footer-links a {
        color: #d1d5db;
        text-decoration: none;
        transition: color 0.2s ease;
    }
    
    .footer-links a:hover {
        color: white;
    }

    /* Responsive Design */
    @media (max-width: 768px) {
        .tos-header h1 {
            font-size: 2rem;
        }
        
        .subtitle {
            font-size: 1.1rem;
        }
        
        .nav-content {
            gap: 15px;
        }
        
        .nav-link {
            padding: 6px 12px;
            font-size: 0.9rem;
        }
        
        .section-card {
            padding: 25px;
        }
        
        .section-card h2 {
            font-size: 2rem;
        }
        
        .subsection h3 {
            font-size: 1.3rem;
        }
        
        .cookie-types,
        .rights-grid,
        .contact-grid {
            grid-template-columns: 1fr;
        }
    }
    
    @media (max-width: 480px) {
        .tos-header {
            padding: 40px 15px;
        }
        
        .tos-header h1 {
            font-size: 1.75rem;
        }
        
        .content-section {
            padding: 20px 15px;
        }
        
        .section-card {
            padding: 20px;
        }
        
        .nav-content {
            justify-content: flex-start;
            overflow-x: auto;
            padding: 10px 15px;
        }
    }
</style> 