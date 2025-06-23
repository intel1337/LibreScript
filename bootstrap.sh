#!/bin/bash

RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

cleanup() {
    echo -e "\n${YELLOW}Shutting down LibreScript services...${NC}"
    kill $FRONTEND_PID $BACKEND_PID $AI_PID 2>/dev/null
    echo -e "${GREEN}All services stopped.${NC}"
    exit 0
}

trap cleanup SIGINT SIGTERM

if ! command -v node &> /dev/null; then
    echo -e "${RED}❌ Node.js is not installed${NC}"
    exit 1
fi

if ! command -v dotnet &> /dev/null; then
    echo -e "${RED}❌ .NET is not installed${NC}"
    exit 1
fi

if ! command -v python3 &> /dev/null; then
    echo -e "${RED}❌ Python3 is not installed${NC}"
    exit 1
fi
cd frontend-web
npm install > /dev/null 2>&1
npm run dev &
cd ../backend-api
dotnet run > /dev/null 2>&1 &
BACKEND_PID=$!
cd ../ai-service
pip3 install -r requirements.txt > /dev/null 2>&1
python3 api.py &
AI_PID=$!
wait $FRONTEND_PID $BACKEND_PID $AI_PID