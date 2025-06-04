import random
from datetime import datetime, timedelta

# Catégories et utilisateurs fictifs (à adapter selon votre base)
categories = [1, 2, 3, 4, 5]  # IDs de catégories existantes
users = [1, 2, 3, 4, 5]       # IDs d'utilisateurs existants

titles = [
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
    "Quels dossiers pour séparer front, back, et docs dans un projet ?"
]

def random_date():
    now = datetime.utcnow()
    delta = timedelta(days=random.randint(0, 60), hours=random.randint(0, 23), minutes=random.randint(0, 59))
    return (now - delta).strftime('%Y-%m-%dT%H:%M:%SZ')

print("INSERT INTO Posts (Title, Content, CategoryId, UserId, CreatedAt, Upvotes, Downvotes) VALUES")
values = []
for i in range(20):
    title = random.choice(titles)
    content = random.choice(contents)
    category = random.choice(categories)
    user = random.choice(users)
    created_at = random_date()
    upvotes = random.randint(0, 30)
    downvotes = random.randint(0, 10)
    values.append(f"('{title.replace("'", "''")}', '{content.replace("'", "''")}', {category}, {user}, '{created_at}', {upvotes}, {downvotes})")

print(",\n".join(values) + ";")
