// aiService.js - Service pour communiquer avec l'API IA via le backend .NET

const API_BASE_URL = 'http://localhost:5028/api';

/**
 * Vérifier le statut de l'API IA
 */
export async function getAiStatus() {
    try {
        const response = await fetch(`${API_BASE_URL}/ai/status`);
        
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        return await response.json();
    } catch (error) {
        console.error('Erreur lors de la vérification du statut IA:', error);
        throw error;
    }
}

/**
 * Vérifier la connectivité avec l'API IA
 */
export async function checkAiHealth() {
    try {
        const response = await fetch(`${API_BASE_URL}/ai/health`);
        
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        return await response.json();
    } catch (error) {
        console.error('Erreur lors de la vérification de la santé IA:', error);
        throw error;
    }
}

/**
 * Générer une réponse avec l'IA
 * @param {string} prompt - Le prompt à envoyer à l'IA
 * @param {Object} options - Options de génération
 * @param {number} options.length - Longueur de la réponse (50-500)
 * @param {number} options.temperature - Température de génération (0.1-1.0)
 * @param {string} token - Token JWT pour l'authentification
 */
export async function generateAiResponse(prompt, options = {}, token) {
    try {
        if (!prompt || prompt.trim() === '') {
            throw new Error('Le prompt ne peut pas être vide');
        }

        if (!token) {
            throw new Error('Token d\'authentification requis');
        }

        const requestData = {
            prompt: prompt.trim(),
            length: options.length || 200,
            temperature: options.temperature || 0.7
        };

        const response = await fetch(`${API_BASE_URL}/ai/generate`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(requestData)
        });

        const data = await response.json();

        if (!response.ok) {
            throw new Error(data.error || `HTTP error! status: ${response.status}`);
        }

        return data;
    } catch (error) {
        console.error('Erreur lors de la génération de réponse IA:', error);
        throw error;
    }
}

/**
 * Recharger le modèle IA
 * @param {string} token - Token JWT pour l'authentification
 */
export async function reloadAiModel(token) {
    try {
        if (!token) {
            throw new Error('Token d\'authentification requis');
        }

        const response = await fetch(`${API_BASE_URL}/ai/reload`, {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });

        const data = await response.json();

        if (!response.ok) {
            throw new Error(data.error || `HTTP error! status: ${response.status}`);
        }

        return data;
    } catch (error) {
        console.error('Erreur lors du rechargement du modèle IA:', error);
        throw error;
    }
}

/**
 * Formater un prompt pour l'IA selon le contexte LibreScript
 * @param {string} question - La question de l'utilisateur
 * @param {string} language - Le langage de programmation (optionnel)
 * @param {string} context - Contexte supplémentaire (optionnel)
 */
export function formatPromptForLibreScript(question, language = '', context = '') {
    let prompt = '';
    
    if (language) {
        prompt += `# Language: ${language}\n`;
    }
    
    prompt += `# Question: ${question}\n`;
    
    if (context) {
        prompt += `# Content: ${context}\n`;
    }
    
    return prompt;
}

/**
 * Valider les paramètres de génération
 * @param {Object} options - Options à valider
 */
export function validateGenerationOptions(options) {
    const errors = [];
    
    if (options.length !== undefined) {
        if (typeof options.length !== 'number' || options.length < 50 || options.length > 500) {
            errors.push('La longueur doit être un nombre entre 50 et 500');
        }
    }
    
    if (options.temperature !== undefined) {
        if (typeof options.temperature !== 'number' || options.temperature < 0.1 || options.temperature > 1.0) {
            errors.push('La température doit être un nombre entre 0.1 et 1.0');
        }
    }
    
    return errors;
}

/**
 * Obtenir des suggestions de prompts basées sur le contexte
 */
export function getPromptSuggestions() {
    return [
        {
            title: "Question Python",
            prompt: "Comment implémenter une fonction de tri rapide en Python ?",
            language: "python"
        },
        {
            title: "Question JavaScript",
            prompt: "Exemple d'utilisation d'async/await en JavaScript",
            language: "javascript"
        },
        {
            title: "Question SQL",
            prompt: "Comment faire une requête JOIN entre deux tables ?",
            language: "sql"
        },
        {
            title: "Question C#",
            prompt: "Comment gérer les exceptions en C# ?",
            language: "csharp"
        },
        {
            title: "Question React",
            prompt: "Comment créer un composant React avec hooks ?",
            language: "javascript"
        },
        {
            title: "Question Java",
            prompt: "Implémentation d'un algorithme de recherche binaire en Java",
            language: "java"
        }
    ];
} 