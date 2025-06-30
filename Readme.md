<img width="800" alt="image" src="https://github.com/user-attachments/assets/9fe540e0-349a-4a6b-a279-696c2747c0aa" />

# LibreScript

LibreScript est une application web / de bureau moderne développée avec une architecture full-stack, combinant un backend .NET et un frontend Svelte.
<img width="1458" alt="image" src="https://github.com/user-attachments/assets/0a017160-2046-45c7-ba55-efa6cd200d8d" />

# Pourquoi ce Projet ?
Tout simplement car c'est une architecture intéressante à coder, qui se démarque des usuels blogs / boutiques en ligne. La performance est aussi demandée et l'optimisation est nécessaire nativement dans le projet. C'est pour ca que J'utilise ASP.NET et Svelte 2 des meilleurs Frameworks Web en termes de performances pures.

## Architecture du Projet

### Backend (.NET)
Le backend est développé en C# avec .NET et comprend :
- Une API RESTful
- Une base de données avec Entity Framework Core
- Des migrations pour la gestion du schéma de base de données
- Une configuration Docker pour le déploiement
- Serveur SMTP via Gmail
### Frontend (Svelte)
Le frontend est développé avec Svelte et comprend :
- Une interface utilisateur moderne et réactive
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
cd backend-api
dotnet restore
dotnet build
dotnet run
```

### Frontend
```bash
cd frontend-web
npm i #"npm ci" in prod
npm run build #prod
npm run dev #"npm run preview" in prod
```

## Démarrage

### Backend
```bash
cd backend-api
dotnet build
dotnet run
```

### Frontend
```bash
cd frontend-web
npm run preview
```

## Déploiement

### Docker
Le projet inclut des configurations Docker pour le déploiement :
- `Dockerfile` pour le backend
- Configuration Kubernetes dans le dossier `k8s`

### CI/CD
Le projet inclut des configurations GitHub Actions pour l'intégration et le déploiement continus.
```
Librescript/
    backend-api/
    ├── Controllers/          # Contrôleurs API
    ├── Models/              # Modèles de données (User.cs, Post.cs, etc.)
    ├── Data/                # Contexte de base de données
    ├── Migrations/          # Migrations EF Core
    ├── Services/            # Services métier (Mail, Verification)
    ├── Tests/               # Tests unitaires
    └── [fichiers config]    # Program.cs, .csproj, appsettings.json
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
<img width="800" alt="image" src="https://github.com/user-attachments/assets/a62d90b7-4581-4242-96e7-807ca649c25d" />

