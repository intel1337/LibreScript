const API_BASE_URL = 'http://192.168.10.106:5028/api';
import { marked } from 'marked';

export const aiService = {
    renderMarkdown(content) {
        if (!content) return '';
        try {
            // Essayer différentes syntaxes selon la version de marked
            if (typeof marked.parse === 'function') {
                return marked.parse(content, { breaks: true, gfm: true });
            } else if (typeof marked === 'function') {
                return marked(content, { breaks: true, gfm: true });
            } else {
                console.error('marked function not found');
                return content;
            }
        } catch (error) {
            console.error('Error rendering markdown:', error, 'Content:', content);
            // Fallback: retourner le contenu brut si le markdown échoue
            return content.replace(/\n/g, '<br>');
        }
    },
    async testConnection() {
        try {
            console.log('Test de connexion vers:', `${API_BASE_URL}/ai/test`);
            const response = await fetch(`${API_BASE_URL}/ai/test`);
            const data = await response.json();
            console.log('Test de connexion réussi:', data);
            return data;
        } catch (error) {
            console.error('Erreur lors du test de connexion:', error);
            throw error;
        }
    },

    async askQuestion(question) {
        try {
            console.log('Envoi de la question:', question);
            console.log('URL utilisée:', `${API_BASE_URL}/ai/ask`);
            
            const response = await fetch(`${API_BASE_URL}/ai/ask`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(question)
            });

            console.log('Status de la réponse:', response.status);
            console.log('Headers de la réponse:', response.headers);

            // Vérifier le content-type de la réponse
            const contentType = response.headers.get('content-type');
            console.log('Content-Type:', contentType);

            // Lire la réponse comme texte d'abord pour diagnostiquer
            const responseText = await response.text();
            console.log('Réponse brute:', responseText);

            if (!response.ok) {
                throw new Error(`Erreur HTTP ${response.status}: ${responseText}`);
            }

            // Essayer de parser le JSON seulement si on a du contenu
            if (!responseText || responseText.trim() === '') {
                throw new Error('Réponse vide du serveur');
            }

            let data;
            try {
                data = JSON.parse(responseText);
            } catch (parseError) {
                console.error('Erreur de parsing JSON:', parseError);
                throw new Error(`Réponse non-JSON du serveur: ${responseText}`);
            }

            return data.answer;
        } catch (error) {
            console.error('Erreur aiService:', error);
            throw error;
        }
    }
};
