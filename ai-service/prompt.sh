#!/bin/bash

# LibreScript AI Prompt Tool - Launcher
echo "LibreScript AI Prompt Tool"
echo "=========================="

cd "$(dirname "$0")"

# Check if Python is installed
if ! command -v python3 &> /dev/null; then
    echo "Error: Python3 is not installed"
    exit 1
fi

# Launch the prompt tool
python3 prompt.py "$@" 