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

minikube service frontend-service-external
minikube service backend-service-external

echo "Frontend: $(minikube service frontend-service-external --url)"
echo "Backend: $(minikube service backend-service-external --url)"
fi


# Build des images Docker
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
clear
minikube service frontend-service-external
minikube service backend-service-external

echo "Frontend: $(minikube service frontend-service-external --url)"
echo "Backend: $(minikube service backend-service-external --url)"
