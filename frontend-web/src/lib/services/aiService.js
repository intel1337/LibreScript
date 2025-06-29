import { BaseService } from './BaseService.js';
import { marked } from 'marked';

/**
 * Service IA pour le chatbot
 */
class AiService extends BaseService {
    constructor() {
        super('AiService');
    }

    /**
     * @param {string} content
     * @returns {string} html
     */
    renderMarkdown(content) {
        if (!content) return '';
        try {
            if (typeof marked.parse === 'function') {
                return marked.parse(content, { breaks: true, gfm: true });
            } else if (typeof marked === 'function') {
                return marked(content, { breaks: true, gfm: true });
            }
            return marked(content);
        } catch (error) {
            this.logError('Error rendering markdown', { error, content });
            return content.replace(/\n/g, '<br>');
        }
    }

    /**
     * @returns {Promise<any>} test de connexion
     */
    async testConnection() {
        try {
            return await this.get('/api/ai/test');
        } catch (error) {
            this.logError('Connection test failed', error);
            throw error;
        }
    }

    /**
     * @param {string} question - La question à poser
     * @returns {Promise<string>} Réponse de l'IA
     */
    async askQuestion(question) {
        if (!question || !question.trim()) {
            throw new Error('Question cannot be empty');
        }

        try {
            const response = await this.post('/api/ai/ask', { question: question.trim() });

            if (typeof response === 'object' && response.answer) {
                return response.answer;
            } else if (typeof response === 'string') {
                return response;
            }

            throw new Error('Invalid response format from AI service');
        } catch (error) {
            this.logError('Failed to ask question', error);
            throw error;
        }
    }

    /**
     * @param {string} url - URL de la requête
     * @param {Object} options - Options de la requête
     * @returns {Promise<any>} Réponse de la requête
     */
    async makeRequest(url, options = {}) {
        try {
            const fullUrl = url.startsWith('http') ? url : this.baseUrl + url;

            const response = await fetch(fullUrl, {
                headers: this.getHeaders(false),
                ...options
            });

            if (!response.ok) {
                throw new Error('Request failed: ' + response.status);
            }

            const responseText = await response.text();

            if (!responseText || responseText.trim() === '') {
                throw new Error('Empty response from server');
            }

            const contentType = response.headers.get('content-type');
            if (contentType && contentType.includes('application/json')) {
                try {
                    return JSON.parse(responseText);
                } catch (parseError) {
                    this.logError('Failed to parse JSON response', parseError);
                    return responseText;
                }
            }

            return responseText;
        } catch (error) {
            this.logError('AI request failed', error);
            throw error;
        }
    }
}

const aiServiceInstance = new AiService();

export const aiService = {
    renderMarkdown: (content) => aiServiceInstance.renderMarkdown(content),
    testConnection: () => aiServiceInstance.testConnection(),
    askQuestion: (question) => aiServiceInstance.askQuestion(question)
};

// Instance Serveur IA

export { AiService };
