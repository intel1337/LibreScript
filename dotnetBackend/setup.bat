@echo off
setlocal

REM Vérifie si dotnet est installé
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo .NET non trouvé, tentative d'installation avec winget...

    REM Vérifie si winget est disponible
    winget --version >nul 2>&1
    if %errorlevel% neq 0 (
        echo Winget n'est pas disponible. Veuillez installer .NET manuellement depuis :
        echo https://dotnet.microsoft.com/download/dotnet/9.0
        exit /b 1
    )

    winget install --id Microsoft.DotNet.SDK.9 --source winget -e
    if %errorlevel% neq 0 (
        echo Échec de l'installation de .NET via winget.
        echo Essayez de l'installer manuellement ici :
        echo https://dotnet.microsoft.com/download/dotnet/9.0
        exit /b 1
    )
)

echo .NET est installé avec succès.
dotnet --version

REM Vérifie si Docker est installé
docker --version >nul 2>&1
if %errorlevel% neq 0 (
    echo Docker n'est pas installé ou non accessible.
    echo Veuillez l'installer depuis https://www.docker.com/
    exit /b 1
)

REM Démarre Docker Desktop (si nécessaire)
echo Tentative de démarrage de Docker Desktop...
start "" "C:\Program Files\Docker\Docker\Docker Desktop.exe"
timeout /t 10 >nul

REM Lance un conteneur PostgreSQL
docker run --name pg-container -e POSTGRES_PASSWORD=mysecret -p 5432:5432 -d postgres

REM Affiche les conteneurs actifs
docker ps

REM Restaure les dépendances du projet .NET
dotnet restore

REM Exécute le projet .NET
dotnet run

endlocal
pause
