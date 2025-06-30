<img width="204" alt="image" src="https://github.com/user-attachments/assets/9fe540e0-349a-4a6b-a279-696c2747c0aa" />

# LibreScript

LibreScript est une application web / de bureau moderne développée avec une architecture full-stack, combinant un backend .NET et un frontend Svelte.

## Architecture du Projet

### Backend (.NET)
Le backend est développé en C# avec .NET et comprend :
- Une API RESTful
- Une base de données avec Entity Framework Core
- Des migrations pour la gestion du schéma de base de données
- Une configuration Docker pour le déploiement
- Une configuration Kubernetes pour l'orchestration

### Frontend (Svelte)
Le frontend est développé avec Svelte et comprend :
- Une interface utilisateur moderne et réactive
- Une intégration Electron pour le déploiement en tant qu'application de bureau
- Une Intégration Web / Web Mobile
- Une configuration de build optimisée
- Des outils de développement modernes (ESLint, Vite, prettier)

## Prérequis

- .NET SDK 8.0 ou supérieur
- Node.js 18 ou supérieur
- Docker (optionnel, pour le déploiement)
- Kubernetes (optionnel, pour l'orchestration)

## Installation

### Backend
```bash
cd dotnetBackend
# Mac, Linux, BSD : ./setup.sh
# Windows :  start setup.bat
dotnet restore
# Optional : dotnet build 
# Ne Pas oublier de setup une db Postgres et l'url dans appsettings.json (Si docker utiliser une image postgres Docker)
```

### Frontend
```bash
cd svelteFrontEnd
npm install
```

## Démarrage

### Backend
```bash
cd dotnetBackend
dotnet run
```

### Frontend
```bash
cd svelteFrontEnd
npm run dev

```

## Déploiement

### Docker
Le projet inclut des configurations Docker pour le déploiement :
- `Dockerfile` pour le backend
- Configuration Kubernetes dans le dossier `k8s`

### CI/CD
Le projet inclut des configurations GitHub Actions pour l'intégration et le déploiement continus.

## Structure des Dossiers

```
LibreScript/
├── dotnetBackend/          # Backend .NET
│   ├── Controllers/       # Contrôleurs API
│   ├── Models/           # Modèles de données
│   ├── Data/             # Contexte de base de données
│   ├── Migrations/       # Migrations EF Core
│   └── k8s/              # Configurations Kubernetes
│
└── svelteFrontEnd/        # Frontend Svelte
    ├── src/              # Code source
    ├── static/           # Ressources statiques
    └── electron.mjs      # Configuration Electron
```

## Choix de la Stack Technique

### Pourquoi .NET pour le Backend ?
- **Performance et Scalabilité** : .NET est reconnu pour ses performances exceptionnelles et sa capacité à gérer des charges élevées, crucial pour un forum avec de nombreux utilisateurs simultanés
- **Entity Framework Core** : Offre une gestion efficace des données avec des capacités de mise en cache avancées et une optimisation des requêtes
- **Support natif de l'asynchrone** : Permet de gérer efficacement les opérations concurrentes et d'optimiser l'utilisation des ressources
- **Sécurité robuste** : Intégration native avec des fonctionnalités de sécurité avancées pour protéger les données des utilisateurs
- **Conteneurisation** : Support natif de Docker et Kubernetes pour une scalabilité horizontale facile

### Pourquoi Svelte pour le Frontend ?
- **Performance** : Svelte compile le code en JavaScript vanilla ultra-optimisé, offrant des performances supérieures aux frameworks traditionnels
- **Taille réduite** : Le bundle final est plus léger, ce qui améliore les temps de chargement et l'expérience utilisateur
- **Réactivité** : Gestion efficace des mises à jour du DOM, cruciale pour un forum avec des mises à jour en temps réel
- **Electron** : Permet de créer une application de bureau native tout en réutilisant le code web, offrant une expérience utilisateur cohérente
- **SEO-friendly** : Facilite l'indexation du contenu du forum par les moteurs de recherche

### Architecture Scalable
- **Microservices** : Architecture modulaire permettant une scalabilité indépendante des composants
- **Load Balancing** : Distribution intelligente de la charge entre les serveurs
- **Caching** : Mise en cache à plusieurs niveaux pour optimiser les performances
- **Monitoring** : Outils de surveillance en temps réel pour détecter et résoudre les goulots d'étranglement


## Contribution

1. Fork le projet
2. Créez une branche pour votre fonctionnalité
3. Committez vos changements
4. Poussez vers la branche
5. Ouvrez une Pull Request

## Licence

Ce projet est sous licence libre.
<img width="204" alt="image" src="https://github.com/user-attachments/assets/a62d90b7-4581-4242-96e7-807ca649c25d" />

