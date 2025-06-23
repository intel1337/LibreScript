"""
LibreScript AI Service - Fine-tuning GPT-2 for code generation
Uses LibreScript posts and comments data to train a model

Usage:
    python ai.py                    # Train from existing checkpoints (or fresh if none)
    python ai.py --fresh            # Force training from fresh (ignore checkpoints)
    python ai.py --generate-only    # Skip training and only generate text
    python ai.py --steps 2000       # Specify number of training steps
    python ai.py --info             # Show checkpoint information
"""

import gpt_2_simple as gpt2
import tensorflow as tf
import requests
import os
import json
import sys
import argparse

# LibreScript API Configuration
API_BASE_URL = "http://localhost:5028/api"
POSTS_ENDPOINT = f"{API_BASE_URL}/post"
COMMENTS_ENDPOINT = f"{API_BASE_URL}/comment"

# Model Configuration   
MODEL_NAME = "124M"
DATA_FILE = "librescript_dataset.txt"
RUN_NAME = "librescript_code_model"
TRAINING_STEPS = 1000

def fetch_posts():
    """Fetch all posts from LibreScript API"""
    print("Fetching posts from LibreScript API...")
    try:
        response = requests.get(POSTS_ENDPOINT)
        if response.status_code == 200:
            return response.json()
        else:
            print(f"Error fetching posts: {response.status_code}")
            return []
    except Exception as e:
        print(f"API connection error: {e}")
        return []

def fetch_comments_for_post(post_id):
    """Fetch all comments for a given post"""
    try:
        response = requests.get(f"{COMMENTS_ENDPOINT}/post/{post_id}")
        if response.status_code == 200:
            return response.json()
        else:
            return []
    except Exception as e:
        print(f"Error fetching comments for post {post_id}: {e}")
        return []

def extract_code_content(posts):
    """Extract and format code content from posts and comments"""
    print("Extracting code content...")
    code_content = []
    
    for post in posts:
        # Ajouter le titre et contenu du post
        post_content = f"# Question: {post.get('title', '')}\n"
        post_content += f"# Content: {post.get('content', '')}\n"
        post_content += f"# Language: {post.get('language', 'other')}\n"
        post_content += f"# Status: {post.get('status', 'Open')}\n\n"
        
        # Récupérer les commentaires/réponses pour ce post
        comments = fetch_comments_for_post(post.get('id'))
        
        if comments:
            post_content += "# Answers:\n"
            for comment in comments:
                post_content += f"## Answer by user {comment.get('userId', 'unknown')}:\n"
                post_content += f"{comment.get('content', '')}\n\n"
                
                # If the comment has replies
                if comment.get('replies'):
                    for reply in comment['replies']:
                        post_content += f"### Reply by user {reply.get('userId', 'unknown')}:\n"
                        post_content += f"{reply.get('content', '')}\n\n"
        
        post_content += "\n" + "="*80 + "\n\n"
        code_content.append(post_content)
    
    return "\n".join(code_content)

def get_checkpoint_info():
    """Retrieve information about existing checkpoints"""
    checkpoint_dir = os.path.join("checkpoint", RUN_NAME)
    
    if not os.path.exists(checkpoint_dir):
        return None
    
    checkpoint_file = os.path.join(checkpoint_dir, "checkpoint")
    counter_file = os.path.join(checkpoint_dir, "counter")
    
    info = {
        "checkpoint_dir": checkpoint_dir,
        "exists": True,
        "latest_model": None,
        "current_step": 0,
        "files": []
    }
    
    # Read checkpoint file to find the most recent model
    if os.path.exists(checkpoint_file):
        try:
            with open(checkpoint_file, 'r') as f:
                content = f.read()
                for line in content.split('\n'):
                    if line.startswith('model_checkpoint_path:'):
                        info["latest_model"] = line.split('"')[1]
                        break
        except Exception as e:
            print(f"Error reading checkpoint file: {e}")
    
    # Read step counter
    if os.path.exists(counter_file):
        try:
            with open(counter_file, 'r') as f:
                info["current_step"] = int(f.read().strip())
        except Exception as e:
            print(f"Error reading counter: {e}")
    
    # List checkpoint files
    if os.path.exists(checkpoint_dir):
        info["files"] = os.listdir(checkpoint_dir)
    
    return info

def prepare_training_data():
    """Prepare training data from LibreScript"""
    posts = fetch_posts()
    
    if not posts:
        raise Exception("No posts retrieved from LibreScript API")
    
    print(f"Retrieved {len(posts)} posts from LibreScript")
    
    # Extract code content
    training_data = extract_code_content(posts)
    
    # Save to file
    with open(DATA_FILE, "w", encoding="utf-8") as f:
        f.write(training_data)
    
    print(f"Training data saved to {DATA_FILE}")
    print(f"Dataset size: {len(training_data)} characters")

def main():
    """Main function for training and generation"""
    
    # Parse command line arguments
    parser = argparse.ArgumentParser(description='LibreScript AI Training')
    parser.add_argument('--fresh', action='store_true', 
                       help='Force restart training from fresh (ignore checkpoints)')
    parser.add_argument('--generate-only', action='store_true',
                       help='Skip training and only generate text')
    parser.add_argument('--steps', type=int, default=TRAINING_STEPS,
                       help=f'Number of training steps (default: {TRAINING_STEPS})')
    parser.add_argument('--info', action='store_true',
                       help='Show checkpoint information and exit')
    
    args = parser.parse_args()
    
    # If --info option is enabled, show information and exit
    if args.info:
        print("LibreScript AI Checkpoint Information")
        print("=" * 50)
        
        checkpoint_info = get_checkpoint_info()
        if checkpoint_info and checkpoint_info["exists"]:
            print(f"Directory: {checkpoint_info['checkpoint_dir']}")
            print(f"Latest model: {checkpoint_info['latest_model']}")
            print(f"Current step: {checkpoint_info['current_step']}")
            print(f"Number of files: {len(checkpoint_info['files'])}")
            print(f"Available files:")
            for file in sorted(checkpoint_info['files']):
                file_path = os.path.join(checkpoint_info['checkpoint_dir'], file)
                if os.path.isfile(file_path):
                    size = os.path.getsize(file_path)
                    if size > 1024*1024:  # MB
                        size_str = f"{size/(1024*1024):.1f} MB"
                    elif size > 1024:  # KB
                        size_str = f"{size/1024:.1f} KB"
                    else:
                        size_str = f"{size} B"
                    print(f"   - {file} ({size_str})")
            
            print(f"\nNext training will resume from step {checkpoint_info['current_step']}")
        else:
            print("No checkpoint found")
            print("Next training will start from scratch")
        
        return
    
    # Download model if necessary
    if not os.path.isdir(os.path.join("models", MODEL_NAME)):
        print(f"Downloading model {MODEL_NAME}...")
        gpt2.download_gpt2(model_name=MODEL_NAME)
    
    # Start TensorFlow session
    print("Starting TensorFlow session...")
    sess = gpt2.start_tf_sess()
    
    if not args.generate_only:
        # Prepare training data
        prepare_training_data()
        
        # Fine-tune the model
        print("Starting fine-tuning...")
        
        # Determine training starting point
        checkpoint_info = get_checkpoint_info()
        
        if args.fresh:
            print("--fresh mode enabled, starting from scratch...")
            restore_from = 'fresh'
        elif checkpoint_info and checkpoint_info["exists"]:
            print(f"Checkpoints found in {checkpoint_info['checkpoint_dir']}")
            print(f"Latest model: {checkpoint_info['latest_model']}")
            print(f"Current step: {checkpoint_info['current_step']}")
            print(f"Available files: {len(checkpoint_info['files'])} files")
            print("Resuming training from last checkpoint...")
            restore_from = 'latest'
        else:
            print("No checkpoint found, starting from scratch...")
            restore_from = 'fresh'
        
        gpt2.finetune(sess,
                      dataset=DATA_FILE,
                      model_name=MODEL_NAME,
                      run_name=RUN_NAME,
                      steps=args.steps,
                      restore_from=restore_from,
                      print_every=100,
                      sample_every=500,
                      save_every=100)
    
    print("Training completed.")
    
    # LibreScript-based code generation
    print("\n" + "="*50)
    print("LibreScript-based code generation:")
    print("="*50)
    
    # Example prompts based on common question types
    prompts = [
        "# Question: How to implement a function in Python",
        "# Question: JavaScript async/await example",
        "# Question: SQL query to join tables",
        "# Content: I need help with React components"
    ]
    
    for prompt in prompts:
        print(f"\nPrompt: {prompt}")
        print("-" * 40)
        gpt2.generate(sess,
                      run_name=RUN_NAME,
                      length=200,
                      temperature=0.7,
                      prefix=prompt,
                      nsamples=1,
                      batch_size=1)
        print("-" * 40)

if __name__ == "__main__":
    main()
