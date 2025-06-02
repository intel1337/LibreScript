# LibreScript – Projet ASP.NET Core & Svelte

## Présentation
LibreScript est une application web composée d’un backend ASP.NET Core (C#) et d’un frontend Svelte. Elle permet la gestion d’utilisateurs, de posts, de catégories et de commentaires, dans un style blog/forum moderne.
---
## Architecture & Design Pattern
- **Backend** : ASP.NET Core (C#), architecture RESTful, pattern MVC (Model-View-Controller)
- **Frontend** : Svelte (Vite)
- **Base de données** : PostgreSQL (via Entity Framework Core)
- **Séparation des responsabilités** :
  - **Controllers** : gèrent les routes HTTP, la logique métier et la validation des requêtes
  - **Models/Entities** : représentent les tables de la base de données
  - **DbContext** : centralise l’accès à la base de données
---
## Fonctionnalités principales
- **Gestion des utilisateurs** :
  - Inscription, connexion (routes prêtes à être implémentées)
  - Recherche par ID ou username
- **Gestion des posts** :
  - CRUD complet (création, lecture, modification, suppression)
  - Association à un utilisateur
- **Gestion des catégories** :
  - CRUD complet
  - Association de posts à des catégories
- **Gestion des commentaires** :
  - CRUD complet
  - Réponses imbriquées (replies)
  - Association à un post et à un utilisateur
- **CORS configuré** pour permettre l’accès au backend depuis le frontend local
- **Configuration flexible** de la base de données via `appsettings.json`
---
## Design technique
- **Pattern MVC** :
  - Les contrôleurs exposent des routes REST (ex : `/api/post`, `/api/category`...)
  - Les entités (models) sont mappées sur les tables PostgreSQL
  - Le DbContext gère les transactions et le suivi des entités
- **Asynchrone** : la plupart des opérations sont asynchrones pour de meilleures performances
- **Sécurité** : endpoints prêts pour l’ajout d’authentification/autorisation
---
## Lancement du projet
1. **Backend**
   - Configurer la base de données dans `appsettings.json` si besoin
   - Lancer le backend :
     ```bash
     cd dotnetBackend
     dotnet run
     ```
2. **Frontend**
   - Installer les dépendances et lancer le serveur Svelte :
     ```bash
     cd svelteFrontEnd
     npm install
     npm run dev
     ```
---
## Exemples de routes API
- `GET /api/user/get-user/id/{id}` : récupérer un utilisateur par ID
- `POST /api/post` : créer un post
- `PUT /api/category/{id}` : modifier une catégorie
- `DELETE /api/comment/{id}` : supprimer un commentaire
---
## Pour aller plus loin
- Ajouter l’authentification JWT
- Ajouter la pagination, la recherche, le tri
- Améliorer la gestion des erreurs et la validation
- Ajouter des tests unitaires et d’intégration
- **Déployer le projet avec Docker et Kubernetes** :
  - Créer des fichiers Dockerfile pour le backend et le frontend
  - Écrire des manifests Kubernetes (Deployment, Service, Ingress)
  - Utiliser un cluster local (ex : minikube) ou cloud (AKS, GKE, EKS)
  - Gérer la persistance de la base PostgreSQL avec un PersistentVolume
  - Ajouter un fichier README/k8s.md pour la documentation du déploiement

---
Projet réalisé dans le cadre d’un projet de fin d’année.
