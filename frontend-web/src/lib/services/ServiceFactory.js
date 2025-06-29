import { AiService } from './aiService.js';
import { AuthService } from './authService.js';
import { PostService } from './postService.js';
import { LoginService } from './loginService.js';
import { CommentService } from './commentService.js';
import { ProfileService } from './profileService.js';
import { RegisterService } from './registerService.js';
import { VerificationService } from './verificationService.js';
import { TagService } from './tagService.js';
import { NotificationService } from './notificationService.js';
import { NewPostService } from './newPostService.js';

/**
 * ServiceFactory - Centralized service management
 * Provides singleton instances of all services and factory methods
 */
class ServiceFactory {
    constructor() {
        this.services = new Map();
        this.initialized = false;
    }

    /**
     * Initialize all services
     */
    init() {
        if (this.initialized) return;

        // Initialize all service instances
        this.services.set('ai', new AiService());
        this.services.set('auth', new AuthService());
        this.services.set('post', new PostService());
        this.services.set('login', new LoginService());
        this.services.set('comment', new CommentService());
        this.services.set('profile', new ProfileService());
        this.services.set('register', new RegisterService());
        this.services.set('verification', new VerificationService());
        this.services.set('tag', new TagService());
        this.services.set('notification', new NotificationService());
        this.services.set('newpost', new NewPostService());
        
        this.initialized = true;

    }

    /**
     * Get a service by name
     * @param {string} serviceName - Name of the service
     * @returns {BaseService} Service instance
     */
    getService(serviceName) {
        if (!this.initialized) {
            this.init();
        }

        const service = this.services.get(serviceName.toLowerCase());
        if (!service) {
            throw new Error(`Service '${serviceName}' not found. Available services: ${Array.from(this.services.keys()).join(', ')}`);
        }

        return service;
    }

    /**
     * Get AI service instance
     * @returns {AiService}
     */
    get ai() {
        return this.getService('ai');
    }

    /**
     * Get Auth service instance
     * @returns {AuthService}
     */
    get auth() {
        return this.getService('auth');
    }

    /**
     * Get Post service instance
     * @returns {PostService}
     */
    get post() {
        return this.getService('post');
    }

    /**
     * Get Login service instance
     * @returns {LoginService}
     */
    get login() {
        return this.getService('login');
    }

    /**
     * Get Comment service instance
     * @returns {CommentService}
     */
    get comment() {
        return this.getService('comment');
    }

    /**
     * Get Profile service instance
     * @returns {ProfileService}
     */
    get profile() {
        return this.getService('profile');
    }

    /**
     * Get Register service instance
     * @returns {RegisterService}
     */
    get register() {
        return this.getService('register');
    }

    /**
     * Get Verification service instance
     * @returns {VerificationService}
     */
    get verification() {
        return this.getService('verification');
    }

    /**
     * Get Tag service instance
     * @returns {TagService}
     */
    get tag() {
        return this.getService('tag');
    }

    /**
     * Get Notification service instance
     * @returns {NotificationService}
     */
    get notification() {
        return this.getService('notification');
    }

    /**
     * Get NewPost service instance
     * @returns {NewPostService}
     */
    get newpost() {
        return this.getService('newpost');
    }

    /**
     * Check if all services are healthy
     * @returns {Promise<Object>} Health check results
     */
    async healthCheck() {
        const results = {};
        
        for (const [name, service] of this.services) {
            try {
                // Test basic service functionality
                if (typeof service.getToken === 'function') {
                    const hasToken = !!service.getToken();
                    results[name] = { status: 'healthy', hasToken };
                } else {
                    results[name] = { status: 'healthy' };
                }
            } catch (error) {
                results[name] = { status: 'error', error: error.message };
            }
        }

        return results;
    }

    /**
     * Get all service names
     * @returns {Array<string>} List of service names
     */
    getServiceNames() {
        if (!this.initialized) {
            this.init();
        }
        return Array.from(this.services.keys());
    }

    /**
     * Reset all services (useful for testing)
     */
    reset() {
        this.services.clear();
        this.initialized = false;
        console.log('[ServiceFactory] Services reset');
    }

    /**
     * Enable debug mode for all services
     */
    enableDebugMode() {
        if (!this.initialized) {
            this.init();
        }

        for (const [, service] of this.services) {
            if (typeof service.setDebugMode === 'function') {
                service.setDebugMode(true);
            }
        }
        console.log('[ServiceFactory] Debug mode enabled for all services');
    }

    /**
     * Disable debug mode for all services
     */
    disableDebugMode() {
        if (!this.initialized) {
            this.init();
        }

        for (const [, service] of this.services) {
            if (typeof service.setDebugMode === 'function') {
                service.setDebugMode(false);
            }
        }
        console.log('[ServiceFactory] Debug mode disabled for all services');
    }

    /**
     * Get usage statistics for services
     * @returns {Object} Usage statistics
     */
    getUsageStats() {
        const stats = {
            totalServices: this.services.size,
            initialized: this.initialized,
            serviceList: this.getServiceNames(),
            timestamp: new Date().toISOString()
        };

        return stats;
    }
}

// Create singleton instance
const serviceFactory = new ServiceFactory();

// Export singleton instance
export default serviceFactory;

// Export individual services for convenience
export {
    AiService,
    AuthService,
    PostService,
    LoginService,
    CommentService,
    ProfileService,
    RegisterService,
    VerificationService,
    TagService,
    NotificationService,
    NewPostService
};

// Export a convenient way to access services
export const Services = {
    get ai() { return serviceFactory.ai; },
    get auth() { return serviceFactory.auth; },
    get post() { return serviceFactory.post; },
    get login() { return serviceFactory.login; },
    get comment() { return serviceFactory.comment; },
    get profile() { return serviceFactory.profile; },
    get register() { return serviceFactory.register; },
    get verification() { return serviceFactory.verification; },
    get tag() { return serviceFactory.tag; },
    get notification() { return serviceFactory.notification; },
    get newpost() { return serviceFactory.newpost; },
    factory: serviceFactory
}; 