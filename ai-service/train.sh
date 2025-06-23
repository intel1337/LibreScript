#!/bin/bash
set -e

if ! command -v python3 &> /dev/null; then
    exit 1
fi

if ! command -v pip3 &> /dev/null; then
    exit 1
fi

if [ ! -f "requirements.txt" ]; then
    cat > requirements.txt << EOF
gpt-2-simple==0.8.1
tensorflow==2.8.0
requests==2.28.1
EOF
fi

pip3 install -r requirements.txt

echo "1. Train from existing checkpoints"
echo "2. Train from scratch"
echo "3. Generate code only"
echo "4. Custom training"
echo "5. Show checkpoint information"
echo ""

read -p "Choose an option (1-5): " choice

case $choice in
    1)
        python3 ai.py
        ;;
    2)
        read -p "Are you sure? (y/N): " confirm
        if [ "$confirm" = "y" ] || [ "$confirm" = "Y" ]; then
            python3 ai.py --fresh
        else
            exit 0
        fi
        ;;
    3)
        python3 ai.py --generate-only
        ;;
    4)
        read -p "Number of training steps (default: 1000): " steps
        steps=${steps:-1000}
        python3 ai.py --steps $steps
        ;;
    5)
        python3 ai.py --info
        ;;
    *)
        exit 1
        ;;
esac
