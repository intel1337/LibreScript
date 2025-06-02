#!/bin/bash

# Met à jour les paquets et installe le SDK .NET 9.0
sudo apt-get update && \
sudo apt-get install -y dotnet-sdk-9.0

# Vérifie la version de .NET
dotnet --version
if [ $? -ne 0 ]; then
    echo "Échec de l'installation de .NET via APT. Tentative avec Homebrew..."

    # Vérifie si brew est installé
    if ! command -v brew &> /dev/null; then
        echo "Homebrew n'est pas installé. Veuillez l'installer d'abord : https://brew.sh/"
        exit 1
    fi

    brew install --cask dotnet-sdk

    if [ $? -ne 0 ]; then
        echo "Échec de l'installation de .NET via Homebrew."
        echo "Essayez l'installation manuelle : https://dotnet.microsoft.com/download/dotnet/9.0"
        exit 1
    else
        echo "Installation de .NET via Homebrew réussie."
    fi
else
    echo ".NET est installé avec succès."
fi

# Démarre le service Docker
echo "Démarrage de Docker..."
sudo systemctl start docker

# Lance un conteneur PostgreSQL
echo "Lancement du conteneur PostgreSQL..."
docker run --name pg-container -e POSTGRES_PASSWORD=mysecret -p 5432:5432 -d postgres

# Affiche les conteneurs en cours d'exécution
docker ps

# Restaure les dépendances du projet .NET
echo "Restauration des dépendances .NET..."
dotnet restore


