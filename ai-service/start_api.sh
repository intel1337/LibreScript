#!/bin/bash

cd "$(dirname "$0")"

if ! command -v python3 &> /dev/null; then
    exit 1
fi

pip3 install -r requirements.txt

python3 api.py
