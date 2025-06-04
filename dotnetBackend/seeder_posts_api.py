import random
from datetime import datetime, timedelta
import requests

# Config
API_URL = "http://localhost:5028/api/post"  # Route conforme à ton contrôleur
users = [1, 2, 3]  # IDs d'utilisateurs existants (ajusté à 3 users)

titles = [
    "How to implement a binary search algorithm in Python?",
    "Comment utiliser SvelteKit avec .NET Core ?",
    "Problème de CORS sur API REST",
    "Meilleures pratiques pour l'architecture Clean en C#",
    "Déployer une app Docker sur AWS ECS",
    "Comment gérer l'authentification JWT côté front ?",
    "Optimiser les requêtes Entity Framework",
    "Intégrer Flowbite dans Svelte",
    "Générer des seeders pour une base de test",
    "Utiliser les animations natives Svelte",
    "Structurer un projet fullstack moderne"
]

contents = [
    "J'aimerais connecter un front SvelteKit à un backend .NET Core. Des exemples ?",
    "J'ai une erreur CORS en appelant mon API depuis le front, comment la corriger ?",
    "Quels patterns recommandez-vous pour une clean architecture en ASP.NET Core ?",
    "Quelles étapes pour déployer un conteneur Docker sur AWS ECS ?",
    "Comment stocker et vérifier un JWT côté SvelteKit ?",
    "Mon appli est lente avec EF Core, des astuces d'optimisation ?",
    "Flowbite fonctionne-t-il bien avec Svelte ?",
    "Je cherche à générer des données de test pour mes entités, une méthode rapide ?",
    "Comment faire des transitions fluides avec Svelte ?",
    "Quels dossiers pour séparer front, back, et docs dans un projet ?",
    "Voici un exemple de code Python pour la recherche binaire..."
]

languages = ["Python", "C#", "Svelte", "Docker"]
statuses = ["Solved", "Open", "Pending", "Closed"]

def random_date():
    now = datetime.utcnow()
    delta = timedelta(days=random.randint(0, 60), hours=random.randint(0, 23), minutes=random.randint(0, 59))
    return (now - delta).strftime('%Y-%m-%dT%H:%M:%SZ')

for i in range(20):
    data = {
        "title": random.choice(titles),
        "content": random.choice(contents),
        "userId": random.choice(users),
        # Les champs suivants sont initialisés à 0 côté backend, mais on peut les passer pour tester
        "upvotes": 0,
        "downvotes": 0,
        "language": random.choice(languages),
        "status": random.choice(statuses),
        "views": 0,
        # Le backend force CreatedAt à DateTime.UtcNow, mais on peut passer une date pour tester
        "createdAt": random_date()
    }
    r = requests.post(API_URL, json=data)
    print(f"POST {r.status_code}: {data['title']}")
    if r.status_code != 201:
        print(r.text)
