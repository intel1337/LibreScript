# LibreScript

## 🧪 Introduction

**LibreScript** est un projet d'initiation aux backends .NET, conçu pour illustrer la création d'une API RESTful en C# avec ASP.NET Core. Il comprend une structure de base pour la gestion des utilisateurs, l'authentification et la gestion de publications, offrant ainsi une base solide pour des applications web modernes.

## 📂 Structure du projet

- **MonApiBackend** : Contient l'API principale avec les contrôleurs et modèles.
- **Projet Fin d'année c#.sln** : Solution Visual Studio pour une gestion centralisée du projet.

## 🔌 Endpoints API

### Utilisateur

- `GET /api/User/get-user/{id}` : Récupère un utilisateur par son ID.
- `GET /api/User/get-user/{username}` : Récupère un utilisateur par son nom d'utilisateur.
- `POST /api/User/register` : Enregistre un nouvel utilisateur.
- `POST /api/User/login` : Connecte un utilisateur existant.

### Publication

- Le contrôleur `PostController` est actuellement vide et peut être développé pour gérer les publications.

## 🧩 Modèles

Les modèles de données sont définis dans des fichiers tels que `Category.cs`. Ils sont utilisés pour structurer les données échangées via l'API.

## 🚀 Lancer le projet

1. Clonez le dépôt :

   ```bash
   git clone https://github.com/intel1337/librescript.git
   cd librescript
