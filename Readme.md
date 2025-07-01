<img width="800" alt="image" src="https://github.com/user-attachments/assets/9fe540e0-349a-4a6b-a279-696c2747c0aa" />

# LibreScript

### LibreScript is a modern web/desktop application developed with a full-stack architecture, combining a .NET backend and a Svelte frontend.

<img width="1458" alt="image" src="https://github.com/user-attachments/assets/0a017160-2046-45c7-ba55-efa6cd200d8d" />

# Why This Project?
Simply because it's an interesting architecture to code, which stands out from the usual blogs/online stores. Performance is also required and optimization is necessary natively in the project. That's why I use ASP.NET and Svelte 2 of the best Web Frameworks in terms of pure performance.

## 🚀 LibreScript CLI Installation and Usage

### Quick Installation

1. **Clone the project:**
```bash
git clone https://github.com/intel1337/LibreScript.git
```

2. **Navigate to the directory:**
```bash
cd LibreScript
# Modify environment variables in backend-api/appsettings.json and in frontend-web/src/lib/config.js
```

3. **Make scripts executable:**
```bash
chmod +x LibreScriptBootstrap.sh
chmod +x deploy.sh
chmod +x cleanup.sh
```

4. **Configure global alias (optional):**
```bash
# The script automatically configures the alias for bash and zsh
./LibreScriptBootstrap.sh
```

### CLI Usage

Once installed, you can use the following commands:

#### **Main LibreScript CLI**
```bash
# From the project directory
./LibreScriptBootstrap.sh

# Or if you used setup.sh, from anywhere
librescript
```

**CLI Features:**
- ✅ Displays system information
- ✅ Configures environment automatically
- ✅ Installs necessary dependencies
- ✅ Builds the .NET project
- ✅ Configures global alias

#### **Kubernetes Deployment**
```bash
# Complete deployment on minikube/kubernetes
librescript --log

# Cleanup kubernetes resources
librescript --clean
```

#### **Available options**
```bash
librescript --clean    # Cleans the environment
librescript --log      # Shows environment logs  
librescript            # Starts the complete environment
```

### Prerequisites

- Git
- Docker & Docker Desktop
- Kubernetes (minikube recommended)
- .NET SDK 9.0
- Node.js 20+

### Quick Start

```bash
# One-command installation
git clone https://github.com/intel1337/LibreScript.git && \
cd LibreScript && \
chmod +x *.sh && \
./LibreScriptBootstrap.sh
```

## Project Architecture

### Backend (.NET)
The backend is developed in C# with .NET and includes:
- A RESTful API
- A database with Entity Framework Core
- Migrations for database schema management
- Docker configuration for deployment
- SMTP server via Gmail

### Frontend (Svelte)
The frontend is developed with Svelte and includes:
- A modern and reactive user interface
- Web / Web Mobile integration
- Optimized build configuration
- Modern development tools (ESLint, Vite, prettier)

## Prerequisites

- .NET SDK 8.0 or higher
- Node.js 18 or higher
- Docker (optional, for deployment)
- Kubernetes (optional, for orchestration)

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

## Dev Startup

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

## Deployment

### Docker
The project includes Docker configurations for deployment:
- `Dockerfile` for the backend
- Kubernetes configuration in the `k8s` folder

### CI/CD
The project includes GitHub Actions configurations for continuous integration and deployment.

```
Librescript/
    backend-api/
    ├── Controllers/          # API Controllers
    ├── Models/              # Data models (User.cs, Post.cs, etc.)
    ├── Data/                # Database context
    ├── Migrations/          # EF Core migrations
    ├── Services/            # Business services (Mail, Verification)
    ├── Tests/               # Unit tests
    └── [config files]       # Program.cs, .csproj, appsettings.json
```

## Technical Stack Choice

### Why .NET for Backend?
- **Performance and Scalability**: .NET is recognized for its exceptional performance and ability to handle high loads, crucial for a forum with many simultaneous users
- **Entity Framework Core**: Offers efficient data management with advanced caching capabilities and query optimization
- **Native async support**: Allows efficient handling of concurrent operations and optimizes resource usage
- **Robust security**: Native integration with advanced security features to protect user data
- **Containerization**: Native support for Docker and Kubernetes for easy horizontal scalability

### Why Svelte for Frontend?
- **Performance**: Svelte compiles code into ultra-optimized vanilla JavaScript, offering superior performance to traditional frameworks
- **Reduced size**: The final bundle is lighter, improving loading times and user experience
- **Reactivity**: Efficient DOM update management, crucial for a forum with real-time updates
- **SEO-friendly**: Facilitates forum content indexing by search engines

### Scalable Architecture
- **Microservices**: Modular architecture allowing independent component scalability
- **Load Balancing**: Intelligent load distribution between servers
- **Caching**: Multi-level caching to optimize performance
- **Monitoring**: Real-time monitoring tools to detect and resolve bottlenecks

## 🏭 Production Stack and Deployment

### Production Architecture

LibreScript is designed for robust and scalable production deployment with a complete containerized architecture:

#### **🔧 Production Technologies**

**Backend (.NET 9.0)**
- **Runtime**: ASP.NET Core with native container support
- **Database**: PostgreSQL 16 with persistent volumes
- **ORM**: Entity Framework Core with production optimizations
- **Security**: JWT Bearer Authentication + Rate Limiting
- **Health Checks**: Dedicated endpoints for Kubernetes probes

**Frontend (Svelte + nginx)**
- **Framework**: SvelteKit with adapter-static for maximum optimization
- **Web server**: nginx alpine with SPA configuration
- **Build**: Static compilation for optimal performance
- **Caching**: Optimized cache headers for static assets

**Infrastructure**
- **Containerization**: Docker multi-stage builds for optimized images
- **Orchestration**: Kubernetes with production-ready manifests
- **Networking**: Internal ClusterIP services + NodePort for exposure
- **Persistence**: PersistentVolumeClaim for database
- **Monitoring**: Health checks and readiness probes

#### **🚀 Deployment Process**

**1. Containerization**
```bash
# Optimized images with multi-stage builds
docker build -f k8s/db/Dockerfile -t librescript-postgres:latest ./k8s/db/
docker build -f k8s/back/Dockerfile -t librescript-backend:latest .
docker build -f k8s/front/Dockerfile -t librescript-frontend:latest .
```

**2. Kubernetes Deployment**
```bash
# Automated deployment with scripts
./deploy.sh  # Complete deployment
./cleanup.sh # Resource cleanup
```

**3. Production Configuration**

**PostgreSQL**:
- 5Gi persistent volume
- Load-optimized configuration
- Automatic initialization scripts
- Integrated backup and restore

**Backend API**:
- 2 replicas for high availability
- Resource limits: 512Mi RAM, 500m CPU
- Health checks on `/api/health` and `/api/health/ready`
- Secure environment variables

**Frontend**:
- 2 nginx replicas for load distribution
- SPA configuration with fallback to index.html
- Cached assets with optimized headers
- Reduced image size (~53MB)

#### **📊 Performance Metrics**

- **Build time**: ~2-3 minutes for complete stack
- **Image sizes**:
  - Frontend: 53.5MB (nginx + static files)
  - Backend: 297MB (optimized .NET runtime)
  - Database: 457MB (PostgreSQL 16)
- **Minimum resources**: 1GB RAM, 2 CPU cores
- **Scalability**: Native horizontal and vertical support

#### **🔒 Production Security**

- **HTTPS**: Ready for reverse proxy (nginx/traefik)
- **Secrets**: Kubernetes environment variables
- **Network Policies**: Service isolation
- **Image Security**: Images based on Alpine Linux
- **Health Monitoring**: Continuous service monitoring

#### **🛠 Administration Tools**

```bash
# Real-time monitoring
kubectl get pods
kubectl logs -l app=backend
minikube dashboard

# Service management
minikube service frontend-service-external  # Frontend access
minikube service backend-service-external   # API access
kubectl port-forward svc/postgres-service 5432:5432  # DB access

# Rolling updates
kubectl set image deployment/backend-deployment backend=librescript-backend:v2
```

This architecture guarantees **high availability**, **horizontal scalability** and **simplified maintenance** for a robust production environment.

## 🌐 Web Deployment with Domain Name

### **Option 1: nginx Reverse Proxy (Recommended)**

```bash
# 1. Expose Kubernetes services on fixed ports
kubectl patch svc backend-service-external -p '{"spec":{"type":"LoadBalancer"}}'
kubectl patch svc frontend-service-external -p '{"spec":{"type":"LoadBalancer"}}'

# 2. nginx configuration on your web server
# /etc/nginx/sites-available/librescript.com
server {
    listen 80;
    server_name librescript.com www.librescript.com;
    
    # Frontend (User interface)
    location / {
        proxy_pass http://localhost:8080;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
    
    # Backend API
    location /api/ {
        proxy_pass http://localhost:3001/api/;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        
        # CORS headers
        add_header Access-Control-Allow-Origin *;
        add_header Access-Control-Allow-Methods "GET, POST, PUT, DELETE, OPTIONS";
        add_header Access-Control-Allow-Headers "Content-Type, Authorization";
    }
}

# 3. HTTPS with Let's Encrypt
sudo certbot --nginx -d librescript.com -d www.librescript.com
```

### **Option 2: Direct Port Forwarding**

```bash
# On your VPS/Cloud server
# Directly expose Kubernetes services

# Backend on port 3001
kubectl port-forward --address 0.0.0.0 service/backend-service 3001:5028 &

# Frontend on port 8080  
kubectl port-forward --address 0.0.0.0 service/frontend-service 8080:80 &

# DNS configuration
# A    librescript.com      → YOUR_SERVER_IP
# A    www.librescript.com  → YOUR_SERVER_IP
```

### **Option 3: Kubernetes Ingress (Production)**

```yaml
# ingress.yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: librescript-ingress
  annotations:
    nginx.ingress.kubernetes.io/rewrite-target: /
    cert-manager.io/cluster-issuer: "letsencrypt-prod"
spec:
  tls:
  - hosts:
    - librescript.com
    secretName: librescript-tls
  rules:
  - host: librescript.com
    http:
      paths:
      - path: /api
        pathType: Prefix
        backend:
          service:
            name: backend-service
            port:
              number: 5028
      - path: /
        pathType: Prefix
        backend:
          service:
            name: frontend-service
            port:
              number: 80
```

### **Frontend Configuration for Production**

```javascript
// frontend-web/src/lib/config.js
export const API_BASE_URL = 'https://librescript.com/api';
// or
export const API_BASE_URL = 'https://api.librescript.com';
```

### **Complete Example - DigitalOcean/AWS**

```bash
# 1. On your droplet/instance
git clone https://github.com/username/LibreScript.git
cd LibreScript

# 2. Install minikube on server
curl -LO https://storage.googleapis.com/minikube/releases/latest/minikube-linux-amd64
sudo install minikube-linux-amd64 /usr/local/bin/minikube

# 3. Deploy
./LibreScriptBootstrap.sh

# 4. Expose with public IP
kubectl port-forward --address 0.0.0.0 service/backend-service 3001:5028 &
kubectl port-forward --address 0.0.0.0 service/frontend-service 80:80 &

# 5. nginx configuration
server {
    listen 80;
    server_name librescript.com;
    location / {
        proxy_pass http://127.0.0.1:80;
    }
    location /api/ {
        proxy_pass http://127.0.0.1:3001/api/;
    }
}
```

### **DNS Configuration**
```
Type  Name                Value               TTL
A     librescript.com     203.0.113.123      300
A     www.librescript.com 203.0.113.123      300
```

**The simplest**: Use **Option 1** with nginx reverse proxy - it's the production standard! 🚀

## 🔧 Troubleshooting & Common Issues

### Port Forwarding Issues

#### **Problem: "lost connection to pod" Error**
```bash
E0701 13:43:59.441434 6659 portforward.go:424] "Unhandled Error" err="an error occurred forwarding 3001 -> 5028: error forwarding port 5028 to pod eb3f7b1eab52297c6739bfdb9e551b5593983926356f4f5b41c43121d8c8a572, uid : Error response from daemon: No such container: eb3f7b1eab52297c6739bfdb9e551b5593983926356f4f5b41c43121d8c8a572"
error: lost connection to pod
```

**Why it occurs:**
- Kubernetes pods are restarted (deployment rollout, crash, resource limits)
- Port forwarding tries to connect to old container IDs that no longer exist
- Docker containers are cleaned up but kubectl still references old IDs

**Solution:**
```bash
# Kill existing port forwarding processes
pkill -f "kubectl port-forward"

# Restart port forwarding to connect to new pods
kubectl port-forward service/backend-service 3001:5028 &
kubectl port-forward service/frontend-service 8080:80 &
```

#### **Problem: "bind: address already in use"**
```bash
Unable to listen on port 5432: Listeners failed to create with the following errors: [unable to create listener: Error listen tcp4 127.0.0.1:5432: bind: address already in use]
```

**Why it occurs:**
- Local PostgreSQL server is running on the same port
- Previous port forwarding process didn't terminate properly
- Another application is using the port

**Solution:**
```bash
# Check what's using the port
lsof -i :5432
sudo lsof -i :3001
sudo lsof -i :8080

# Kill the process or use different ports
kubectl port-forward service/postgres-service 5433:5432 &  # Use port 5433 instead
kubectl port-forward service/backend-service 3001:5028 &
kubectl port-forward service/frontend-service 8080:80 &
```

### Database Issues

#### **Problem: "relation 'Users' does not exist"**
**Why it occurs:**
- Entity Framework expects exact table names with proper capitalization
- Kubernetes PostgreSQL database is separate from local database
- Migrations were run on local DB instead of Kubernetes DB

**Solution:**
```bash
# Create tables with correct EF Core naming in Kubernetes DB
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

### macOS/Darwin Specific Issues

#### **Problem: Terminal Blocking with minikube service**
**Why it occurs:**
- `minikube service` commands require keeping terminal open on macOS
- Docker driver on Darwin blocks terminal for tunnel connections
- Random ports assigned by minikube tunnel

**Solution:**
Use `kubectl port-forward` instead of `minikube service`:
```bash
# Instead of: minikube service backend-service-external
kubectl port-forward service/backend-service 3001:5028 &

# Instead of: minikube service frontend-service-external  
kubectl port-forward service/frontend-service 8080:80 &
```

### CORS Issues

#### **Problem: "Origin not allowed by Access-Control-Allow-Origin"**
**Why it occurs:**
- Frontend and backend running on different ports
- Port forwarding URLs change when pods restart
- Frontend config points to wrong backend URL

**Solution:**
1. **Update frontend config:**
```javascript
// frontend-web/src/lib/config.js
export const API_BASE_URL = 'http://localhost:3001';
```

2. **Rebuild and redeploy frontend:**
```bash
eval $(minikube docker-env)
docker build -f k8s/front/Dockerfile -t librescript-frontend:latest .
kubectl rollout restart deployment/frontend-deployment
```

### Service Access

#### **Current Working Configuration**
```bash
# Frontend: http://localhost:8080
# Backend API: http://localhost:3001
# Database: Available via kubectl exec (internal only)

# Check services are running:
curl -I http://localhost:8080                    # Frontend (should return 200 OK)
curl http://localhost:3001/api/health            # Backend health check
curl http://localhost:3001/api/post              # API test
```

#### **Complete Service Restart Procedure**
```bash
# 1. Restart all deployments
kubectl rollout restart deployment/backend-deployment
kubectl rollout restart deployment/frontend-deployment
kubectl rollout restart deployment/postgres-deployment

# 2. Wait for deployments to be ready
kubectl rollout status deployment/backend-deployment
kubectl rollout status deployment/frontend-deployment
kubectl rollout status deployment/postgres-deployment

# 3. Kill old port forwarding processes
pkill -f "kubectl port-forward" || true

# 4. Restart port forwarding
kubectl port-forward service/backend-service 3001:5028 &
kubectl port-forward service/frontend-service 8080:80 &

# 5. Verify services
curl -I http://localhost:8080                    # Should return 200 OK
curl http://localhost:3001/api/health            # Should return health status
```

### API Testing

#### **Test POST API (Create Post)**
```bash
# 1. Create a user first
curl -X POST http://localhost:3001/api/user/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "fullName": "Test User", 
    "password": "testpass123",
    "email": "test@example.com"
  }'

# 2. Create a post using the user ID
curl -X POST http://localhost:3001/api/post \
  -H "Content-Type: application/json" \
  -d '{
    "title": "My Test Post",
    "content": "This is a test post content",
    "userId": 1,
    "language": "en",
    "status": "published"
  }'

# 3. Get all posts
curl http://localhost:3001/api/post | jq .

# 4. Get specific post
curl http://localhost:3001/api/post/1 | jq .
```

### Quick Debugging Commands

```bash
# Check pod status
kubectl get pods
kubectl describe pod <pod-name>

# Check services
kubectl get services

# Check logs
kubectl logs deployment/backend-deployment --tail=20
kubectl logs deployment/frontend-deployment --tail=20
kubectl logs deployment/postgres-deployment --tail=20

# Check port forwarding processes
ps aux | grep "kubectl port-forward" | grep -v grep

# Test connectivity
curl -v http://localhost:3001/api/health
curl -v http://localhost:8080

# Database connection test
kubectl exec -it deployment/postgres-deployment -- psql -U testuser -d your_db -c "SELECT tablename FROM pg_tables WHERE schemaname = 'public';"
```

### Performance Issues

#### **Problem: Slow API responses**
**Possible causes:**
- Database connection issues
- Resource limits on containers
- Network connectivity problems

**Solutions:**
```bash
# Check resource usage
kubectl top pods

# Increase resource limits
kubectl patch deployment backend-deployment -p '{"spec":{"template":{"spec":{"containers":[{"name":"backend","resources":{"limits":{"memory":"1Gi","cpu":"1000m"}}}]}}}}'

# Check database performance
kubectl exec -it deployment/postgres-deployment -- psql -U testuser -d your_db -c "SELECT count(*) FROM \"Users\";"
```

## Contribution

1. Fork the project
2. Create a branch for your feature
3. Commit your changes
4. Push to the branch
5. Open a Pull Request

## License

This project is under free license.
<img width="800" alt="image" src="https://github.com/user-attachments/assets/a62d90b7-4581-4242-96e7-807ca649c25d" /> 