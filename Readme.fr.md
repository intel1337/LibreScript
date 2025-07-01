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
# Modifier les Variables d'environnements dans backend-api/appsettings.json et dans frontend-web/src/lib/config.js
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

# Ou si vous avez utilisé le setup.sh, depuis n'importe où
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
librescript --log

# Nettoyage des ressources kubernetes
librescript --clean
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

## 🔧 Dépannage & Problèmes Courants

### Problèmes de Port Forwarding

#### **Problème: Erreur "lost connection to pod"**
```bash
E0701 13:43:59.441434 6659 portforward.go:424] "Unhandled Error" err="an error occurred forwarding 3001 -> 5028: error forwarding port 5028 to pod eb3f7b1eab52297c6739bfdb9e551b5593983926356f4f5b41c43121d8c8a572, uid : Error response from daemon: No such container: eb3f7b1eab52297c6739bfdb9e551b5593983926356f4f5b41c43121d8c8a572"
error: lost connection to pod
```

**Pourquoi cela se produit:**
- Les pods Kubernetes sont redémarrés (déploiement, crash, limites de ressources)
- Le port forwarding tente de se connecter à d'anciens IDs de conteneurs qui n'existent plus
- Les conteneurs Docker sont nettoyés mais kubectl fait encore référence aux anciens IDs


**Solution:**
```bash
# Tuer les processus de port forwarding existants
pkill -f "kubectl port-forward"

# Redémarrer le port forwarding pour se connecter aux nouveaux pods
kubectl port-forward service/backend-service 3001:5028 &
kubectl port-forward service/frontend-service 8080:80 &
```

#### **Problème: "bind: address already in use"**
```bash
Unable to listen on port 5432: Listeners failed to create with the following errors: [unable to create listener: Error listen tcp4 127.0.0.1:5432: bind: address already in use]
```

**Pourquoi cela se produit:**
- Un serveur PostgreSQL local fonctionne sur le même port
- Un processus de port forwarding précédent ne s'est pas terminé correctement
- Une autre application utilise le port

**Solution:**
```bash
# Vérifier ce qui utilise le port
lsof -i :5432
sudo lsof -i :3001
sudo lsof -i :8080

# Tuer le processus ou utiliser des ports différents
kubectl port-forward service/postgres-service 5433:5432 &  # Utiliser le port 5433 à la place
kubectl port-forward service/backend-service 3001:5028 &
kubectl port-forward service/frontend-service 8080:80 &
```

### Problèmes de Base de Données

#### **Problème: "relation 'Users' does not exist"**
**Pourquoi cela se produit:**
- Entity Framework attend des noms de tables exacts avec une capitalisation appropriée
- La base de données PostgreSQL Kubernetes est séparée de la base de données locale
- Les migrations ont été exécutées sur la DB locale au lieu de la DB Kubernetes

**Solution:**
```bash
# Créer les tables avec la nomenclature EF Core correcte dans la DB Kubernetes
kubectl exec deployment/postgres-deployment -- psql -U testuser -d your_db -c "
CREATE TABLE \"Users\" (
    \"Id\" SERIAL PRIMARY KEY,
    \"Username\" TEXT NOT NULL,
    \"FullName\" TEXT NOT NULL,
    \"Password\" TEXT NOT NULL,
    \"Email\" TEXT NOT NULL,
    \"Verified\" BOOLEAN DEFAULT FALSE,
    \"VerificationCode\" TEXT,
    \"VerificationCodeExpiry\" TIMESTAMP WITH TIME ZONE,
    \"CreatedAt\" TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

CREATE TABLE \"Posts\" (
    \"Id\" SERIAL PRIMARY KEY,
    \"Title\" TEXT NOT NULL,
    \"Content\" TEXT NOT NULL,
    \"UserId\" INTEGER NOT NULL,
    \"Upvotes\" INTEGER DEFAULT 0,
    \"Downvotes\" INTEGER DEFAULT 0,
    \"Language\" TEXT DEFAULT '',
    \"Status\" TEXT DEFAULT '',
    \"Views\" DOUBLE PRECISION DEFAULT 0,
    \"CreatedAt\" TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

CREATE TABLE \"Categories\" (
    \"Id\" SERIAL PRIMARY KEY,
    \"Name\" TEXT NOT NULL,
    \"Description\" TEXT NOT NULL,
    \"CreatedAt\" TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

CREATE TABLE \"Comments\" (
    \"Id\" SERIAL PRIMARY KEY,
    \"Content\" TEXT NOT NULL,
    \"PostId\" INTEGER NOT NULL,
    \"UserId\" INTEGER NOT NULL,
    \"CreatedAt\" TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

CREATE TABLE \"PostVotes\" (
    \"Id\" SERIAL PRIMARY KEY,
    \"PostId\" INTEGER NOT NULL,
    \"UserId\" INTEGER NOT NULL,
    \"IsUpvote\" BOOLEAN NOT NULL,
    \"CreatedAt\" TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);
"
```

### Problèmes Spécifiques macOS/Darwin

#### **Problème: Blocage du Terminal avec minikube service**
**Pourquoi cela se produit:**
- Les commandes `minikube service` nécessitent de garder le terminal ouvert sur macOS
- Le driver Docker sur Darwin bloque le terminal pour les connexions tunnel
- Ports aléatoires assignés par le tunnel minikube

**Solution:**
Utiliser `kubectl port-forward` au lieu de `minikube service`:
```bash
# Au lieu de: minikube service backend-service-external
kubectl port-forward service/backend-service 3001:5028 &

# Au lieu de: minikube service frontend-service-external  
kubectl port-forward service/frontend-service 8080:80 &
```

### Problèmes CORS

#### **Problème: "Origin not allowed by Access-Control-Allow-Origin"**
**Pourquoi cela se produit:**
- Frontend et backend fonctionnent sur des ports différents
- Les URLs de port forwarding changent quand les pods redémarrent
- La configuration frontend pointe vers la mauvaise URL backend

**Solution:**
1. **Mettre à jour la configuration frontend:**
```javascript
// frontend-web/src/lib/config.js
export const API_BASE_URL = 'http://localhost:3001';
```

2. **Reconstruire et redéployer le frontend:**
```bash
eval $(minikube docker-env)
docker build -f k8s/front/Dockerfile -t librescript-frontend:latest .
kubectl rollout restart deployment/frontend-deployment
```

### Accès aux Services

#### **Configuration Fonctionnelle Actuelle**
```bash
# Frontend: http://localhost:8080
# API Backend: http://localhost:3001
# Base de données: Disponible via kubectl exec (interne uniquement)

# Vérifier que les services fonctionnent:
curl -I http://localhost:8080                    # Frontend (devrait retourner 200 OK)
curl http://localhost:3001/api/health            # Vérification de santé backend
curl http://localhost:3001/api/post              # Test API
```

#### **Procédure Complète de Redémarrage des Services**
```bash
# 1. Redémarrer tous les déploiements
kubectl rollout restart deployment/backend-deployment
kubectl rollout restart deployment/frontend-deployment
kubectl rollout restart deployment/postgres-deployment

# 2. Attendre que les déploiements soient prêts
kubectl rollout status deployment/backend-deployment
kubectl rollout status deployment/frontend-deployment
kubectl rollout status deployment/postgres-deployment

# 3. Tuer les anciens processus de port forwarding
pkill -f "kubectl port-forward" || true

# 4. Redémarrer le port forwarding
kubectl port-forward service/backend-service 3001:5028 &
kubectl port-forward service/frontend-service 8080:80 &

# 5. Vérifier les services
curl -I http://localhost:8080                    # Devrait retourner 200 OK
curl http://localhost:3001/api/health            # Devrait retourner le statut de santé
```

### Tests API

#### **Test API POST (Créer un Post)**
```bash
# 1. Créer d'abord un utilisateur
curl -X POST http://localhost:3001/api/user/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "fullName": "Test User", 
    "password": "testpass123",
    "email": "test@example.com"
  }'

# 2. Créer un post en utilisant l'ID utilisateur
curl -X POST http://localhost:3001/api/post \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Mon Post de Test",
    "content": "Ceci est le contenu d'un post de test",
    "userId": 1,
    "language": "fr",
    "status": "published"
  }'

# 3. Récupérer tous les posts
curl http://localhost:3001/api/post | jq .

# 4. Récupérer un post spécifique
curl http://localhost:3001/api/post/1 | jq .
```

### Commandes de Débogage Rapide

```bash
# Vérifier le statut des pods
kubectl get pods
kubectl describe pod <nom-du-pod>

# Vérifier les services
kubectl get services

# Vérifier les logs
kubectl logs deployment/backend-deployment --tail=20
kubectl logs deployment/frontend-deployment --tail=20
kubectl logs deployment/postgres-deployment --tail=20

# Vérifier les processus de port forwarding
ps aux | grep "kubectl port-forward" | grep -v grep

# Tester la connectivité
curl -v http://localhost:3001/api/health
curl -v http://localhost:8080

# Test de connexion à la base de données
kubectl exec -it deployment/postgres-deployment -- psql -U testuser -d your_db -c "SELECT tablename FROM pg_tables WHERE schemaname = 'public';"
```

### Problèmes de Performance

#### **Problème: Réponses API lentes**
**Causes possibles:**
- Problèmes de connexion à la base de données
- Limites de ressources sur les conteneurs
- Problèmes de connectivité réseau

**Solutions:**
```bash
# Vérifier l'utilisation des ressources
kubectl top pods

# Augmenter les limites de ressources
kubectl patch deployment backend-deployment -p '{"spec":{"template":{"spec":{"containers":[{"name":"backend","resources":{"limits":{"memory":"1Gi","cpu":"1000m"}}}]}}}}'

# Vérifier les performances de la base de données
kubectl exec -it deployment/postgres-deployment -- psql -U testuser -d your_db -c "SELECT count(*) FROM \"Users\";"
```

## Contribution

1. Fork le projet
2. Créez une branche pour votre fonctionnalité
3. Committez vos changements
4. Poussez vers la branche
5. Ouvrez une Pull Request

## Licence

Ce projet est sous licence libre.
<img width="800" alt="image" src="https://github.com/user-attachments/assets/a62d90b7-4581-4242-96e7-807ca649c25d" />

