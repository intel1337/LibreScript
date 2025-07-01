<img width="800" alt="image" src="https://github.com/user-attachments/assets/9fe540e0-349a-4a6b-a279-696c2747c0aa" />

# LibreScript

### LibreScript est une application web / de bureau moderne développée avec une architecture full-stack, combinant un backend .NET et un frontend Svelte.

<img width="1458" alt="image" src="https://github.com/user-attachments/assets/0a017160-2046-45c7-ba55-efa6cd200d8d" />

# Pourquoi ce Projet ?
Tout simplement car c'est une architecture intéressante à coder, qui se démarque des usuels blogs / boutiques en ligne. La performance est aussi demandée et l'optimisation est nécessaire nativement dans le projet. C'est pour ca que J'utilise ASP.NET et Svelte 2 des meilleurs Frameworks Web en termes de performances pures.

## 🚀 Installation et Utilisation du CLI LibreScript

### Installation Rapide

1. **Cloner le projet :**
```bash
git clone https://github.com/intel1337/LibreScript.git
```

2. **Naviguer dans le répertoire :**
```bash
cd LibreScript
```

3. **Rendre les scripts exécutables :**
```bash
chmod +x LibreScriptBootstrap.sh
chmod +x deploy.sh
chmod +x cleanup.sh
```

4. **Configurer l'alias global (optionnel) :**
```bash
# Le script configure automatiquement l'alias pour bash et zsh
./LibreScriptBootstrap.sh
```

### Utilisation du CLI

Une fois installé, vous pouvez utiliser les commandes suivantes :

#### **LibreScript CLI Principal**
```bash
# Depuis le répertoire du projet
./LibreScriptBootstrap.sh

# Ou si l'alias est configuré, depuis n'importe où
librescript
```

**Fonctionnalités du CLI :**
- ✅ Affiche les informations système
- ✅ Configure l'environnement automatiquement
- ✅ Installe les dépendances nécessaires
- ✅ Build le projet .NET
- ✅ Configure l'alias global

#### **Déploiement Kubernetes**
```bash
# Déploiement complet sur minikube/kubernetes
./deploy.sh

# Nettoyage des ressources kubernetes
./cleanup.sh
```

#### **Options disponibles**
```bash
librescript --clean    # Nettoie l'environnement
librescript --log      # Affiche les logs de l'environnement  
librescript            # Démarre l'environnement complet
```

### Prérequis

- Git
- Docker & Docker Desktop
- Kubernetes (minikube recommandé)
- .NET SDK 9.0
- Node.js 20+

### Démarrage Rapide

```bash
# Installation en une commande
git clone https://github.com/intel1337/LibreScript.git && \
cd LibreScript && \
chmod +x *.sh && \
./LibreScriptBootstrap.sh
```

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

### Database
```bash
psql -U postgres
CREATE USER testuser WITH PASSWORD 'testpass';
CREATE DATABASE your_db;
GRANT ALL PRIVILEGES ON DATABASE your_db TO testuser;
```


### Frontend
```bash
cd frontend-web
npm i #"npm ci" in prod
npm run build #prod
npm run dev #"npm run preview" in prod
```
### k8s
> Build
```bash
# PATH user@bash/Users/your_user/your_folder/LibreScript/$

docker build -f k8s/db/Dockerfile -t librescript-postgres ./k8s/db/
docker build -f k8s/back/Dockerfile -t librescript-backend .
docker build -f k8s/front/Dockerfile -t librescript-frontend .
```
> Run
```bash
docker run -d \
  --name librescript-db \
  -p 5432:5432 \
  -e POSTGRES_PASSWORD=postgres \
  -v postgres_data:/var/lib/postgresql/data \
  librescript-postgres

docker run -p 5028:5028 librescript-backend
docker run -d --name librescript-front -p 8080:80 librescript-frontend
```
> k8s Deployment
```bash
x
```

## Démarrage Dev

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


## 🏭 Stack de Production et Déploiement

### Architecture de Production

LibreScript est conçu pour une mise en production robuste et scalable avec une architecture conteneurisée complète :

#### **🔧 Technologies de Production**

**Backend (.NET 9.0)**
- **Runtime** : ASP.NET Core avec support natif des conteneurs
- **Base de données** : PostgreSQL 16 avec volumes persistants
- **ORM** : Entity Framework Core avec optimisations de production
- **Sécurité** : JWT Bearer Authentication + Rate Limiting
- **Health Checks** : Endpoints dédiés pour Kubernetes probes

**Frontend (Svelte + nginx)**
- **Framework** : SvelteKit avec adapter-static pour optimisation maximale
- **Serveur web** : nginx alpine avec configuration SPA
- **Build** : Compilation statique pour des performances optimales
- **Caching** : Headers de cache optimisés pour les assets statiques

**Infrastructure**
- **Conteneurisation** : Docker multi-stage builds pour des images optimisées
- **Orchestration** : Kubernetes avec manifests production-ready
- **Networking** : Services ClusterIP internes + NodePort pour l'exposition
- **Persistance** : PersistentVolumeClaim pour la base de données
- **Monitoring** : Health checks et readiness probes

#### **🚀 Processus de Déploiement**

**1. Containerisation**
```bash
# Images optimisées avec multi-stage builds
docker build -f k8s/db/Dockerfile -t librescript-postgres:latest ./k8s/db/
docker build -f k8s/back/Dockerfile -t librescript-backend:latest .
docker build -f k8s/front/Dockerfile -t librescript-frontend:latest .
```

**2. Déploiement Kubernetes**
```bash
# Déploiement automatisé avec scripts
./deploy.sh  # Deploy complet
./cleanup.sh # Nettoyage des ressources
```

**3. Configuration de Production**

**PostgreSQL** :
- Volume persistant 5Gi
- Configuration optimisée pour la charge
- Scripts d'initialisation automatiques
- Backup et restore intégrés

**Backend API** :
- 2 répliques pour haute disponibilité
- Resources limits : 512Mi RAM, 500m CPU
- Health checks sur `/api/health` et `/api/health/ready`
- Variables d'environnement sécurisées

**Frontend** :
- 2 répliques nginx pour distribution de charge
- Configuration SPA avec fallback sur index.html
- Assets cachés avec headers optimisés
- Taille d'image réduite (~53MB)

#### **📊 Métriques de Performance**

- **Temps de build** : ~2-3 minutes pour la stack complète
- **Taille des images** :
  - Frontend : 53.5MB (nginx + static files)
  - Backend : 297MB (.NET runtime optimisé)
  - Database : 457MB (PostgreSQL 16)
- **Ressources minimales** : 1GB RAM, 2 CPU cores
- **Scalabilité** : Support natif horizontal et vertical

#### **🔒 Sécurité de Production**

- **HTTPS** : Prêt pour reverse proxy (nginx/traefik)
- **Secrets** : Variables d'environnement Kubernetes
- **Network Policies** : Isolation des services
- **Image Security** : Images basées sur Alpine Linux
- **Health Monitoring** : Surveillance continue des services

#### **🛠 Outils d'Administration**

```bash
# Monitoring en temps réel
kubectl get pods
kubectl logs -l app=backend
minikube dashboard

# Gestion des services
minikube service frontend-service-external  # Accès frontend
minikube service backend-service-external   # Accès API
kubectl port-forward svc/postgres-service 5432:5432  # Accès DB

# Mise à jour rolling
kubectl set image deployment/backend-deployment backend=librescript-backend:v2
```

Cette architecture garantit une **haute disponibilité**, une **scalabilité horizontale** et une **maintenance simplifiée** pour un environnement de production robuste.

## Contribution

1. Fork le projet
2. Créez une branche pour votre fonctionnalité
3. Committez vos changements
4. Poussez vers la branche
5. Ouvrez une Pull Request

## Licence

Ce projet est sous licence libre.
<img width="800" alt="image" src="https://github.com/user-attachments/assets/a62d90b7-4581-4242-96e7-807ca649c25d" />

