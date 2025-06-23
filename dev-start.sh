#!/bin/bash

RED='\033[0;31m'; GREEN='\033[0;32m'; YELLOW='\033[1;33m'
BLUE='\033[0;34m'; PURPLE='\033[0;35m'; CYAN='\033[0;36m'; NC='\033[0m'

FRONTEND_PID=""; BACKEND_PID=""; AI_PID=""

cleanup() {
    [[ $FRONTEND_PID ]] && kill $FRONTEND_PID 2>/dev/null
    [[ $BACKEND_PID ]] && kill $BACKEND_PID 2>/dev/null
    [[ $AI_PID ]] && kill $AI_PID 2>/dev/null
    exit 0
}

trap cleanup SIGINT SIGTERM

check_dependencies() {
    local missing=()
    command -v node >/dev/null || missing+=("Node.js")
    command -v dotnet >/dev/null || missing+=(".NET")
    command -v python3 >/dev/null || missing+=("Python3")
    [ ${#missing[@]} -ne 0 ] && exit 1
}

start_frontend() {
    cd frontend-web || exit 1
    [ ! -d node_modules ] && npm install
    npm run dev > ../logs/frontend.log 2>&1 &
    FRONTEND_PID=$!
    cd ..
}

start_backend() {
    cd backend-api || exit 1
    dotnet run > ../logs/backend.log 2>&1 &
    BACKEND_PID=$!
    cd ..
}

start_ai() {
    cd ai-service || exit 1
    [ ! -f "checkpoint/librescript_code_model/checkpoint" ]
    pip3 install -r requirements.txt > /dev/null 2>&1
    python3 api.py > ../logs/ai.log 2>&1 &
    AI_PID=$!
    cd ..
}

custom_selection() {
    read -p "Start Frontend? (y/N): " start_fe
    read -p "Start Backend? (y/N): " start_be
    read -p "Start AI Service? (y/N): " start_ai_service
    [[ $start_fe =~ ^[Yy]$ ]] && start_frontend
    [[ $start_be =~ ^[Yy]$ ]] && start_backend
    [[ $start_ai_service =~ ^[Yy]$ ]] && start_ai
}

mkdir -p logs
check_dependencies

if [ $# -eq 0 ]; then
    echo -e "1 - All services"
    echo -e "2 - Web only"
    echo -e "3 - Frontend only"
    echo -e "4 - Backend only"
    echo -e "5 - AI only"
    echo -e "6 - Custom"
    echo -e "0 - Exit"
    read -p "Enter choice: " choice
    case $choice in
        1) start_frontend; start_backend; start_ai ;;
        2) start_frontend; start_backend ;;
        3) start_frontend ;;
        4) start_backend ;;
        5) start_ai ;;
        6) custom_selection ;;
        0) exit 0 ;;
        *) exit 1 ;;
    esac
else
    case $1 in
        "all") start_frontend; start_backend; start_ai ;;
        "web") start_frontend; start_backend ;;
        "frontend"|"fe") start_frontend ;;
        "backen
