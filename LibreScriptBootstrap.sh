#!/bin/bash

if [ "$1" == "--clean" ]; then
    kubectl delete -f k8s/db/db-deployment.yml
    kubectl delete -f k8s/back/backend-deployment.yml
    kubectl delete -f k8s/front/frontend-deployment.yml
    kubectl delete -f k8s/front/frontend-service.yml
    kubectl delete -f k8s/back/backend-service.yml
    exit 0
fi

if [ "$1" == "--log" ]; then
    eval $(minikube docker-env)
    docker build -f k8s/db/Dockerfile -t librescript-postgres:latest ./k8s/db/
    docker build -f k8s/back/Dockerfile -t librescript-backend:latest .
    docker build -f k8s/front/Dockerfile -t librescript-frontend:latest .

    kubectl apply -f k8s/db/db-deployment.yml
    kubectl wait --for=condition=available deployment/postgres-deployment --timeout=300s

    kubectl apply -f k8s/back/backend-deployment.yml
    kubectl apply -f k8s/back/backend-service.yml

    kubectl apply -f k8s/front/frontend-deployment.yml
    kubectl apply -f k8s/front/frontend-service.yml

    kubectl wait --for=condition=available deployment/backend-deployment --timeout=300s
    kubectl wait --for=condition=available deployment/frontend-deployment --timeout=300s

    # Start services in background
    minikube service frontend-service-external &
    minikube service backend-service-external &
    
    # Wait a moment for services to start
    sleep 3
    
    # Get URLs
    FRONTEND_URL=$(minikube service frontend-service-external --url)
    BACKEND_URL=$(minikube service backend-service-external --url)
    
    echo "=== LibreScript Services ==="
    echo "Frontend: $FRONTEND_URL"
    echo "Backend API: $BACKEND_URL"
    echo "============================"
    
    # Open in browser (non-blocking)
    if command -v open &> /dev/null; then
        open "$FRONTEND_URL"
    elif command -v xdg-open &> /dev/null; then
        xdg-open "$FRONTEND_URL"
    fi
    
    exit 0
fi

# Build des images Docker
eval $(minikube docker-env)
docker build -f k8s/db/Dockerfile -t librescript-postgres:latest ./k8s/db/
docker build -f k8s/back/Dockerfile -t librescript-backend:latest .
docker build -f k8s/front/Dockerfile -t librescript-frontend:latest .

kubectl apply -f k8s/db/db-deployment.yml
kubectl wait --for=condition=available deployment/postgres-deployment --timeout=300s

kubectl apply -f k8s/back/backend-deployment.yml
kubectl apply -f k8s/back/backend-service.yml

kubectl apply -f k8s/front/frontend-deployment.yml
kubectl apply -f k8s/front/frontend-service.yml

kubectl wait --for=condition=available deployment/backend-deployment --timeout=300s
kubectl wait --for=condition=available deployment/frontend-deployment --timeout=300s

# Start services in background
minikube service frontend-service-external &
minikube service backend-service-external &

# Wait a moment for services to start
sleep 3

# Get URLs
FRONTEND_URL=$(minikube service frontend-service-external --url)
BACKEND_URL=$(minikube service backend-service-external --url)

clear
echo "=== LibreScript Services ==="
echo "Frontend: $FRONTEND_URL"
echo ""
echo "Backend API: $BACKEND_URL"
echo "============================"

# Open in browser (non-blocking)
if command -v open &> /dev/null; then
    open "$FRONTEND_URL"
elif command -v xdg-open &> /dev/null; then
    xdg-open "$FRONTEND_URL"
fi