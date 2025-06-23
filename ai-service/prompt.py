#!/usr/bin/env python3

import gpt_2_simple as gpt2
import tensorflow as tf
import os
import sys

MODEL_NAME = "124M"
RUN_NAME = "librescript_code_model"

def load_model():
    print("Chargement du modèle...")
    
    if not os.path.isdir(os.path.join("models", MODEL_NAME)):
        print(f"Downloading base model {MODEL_NAME}...")
        gpt2.download_gpt2(model_name=MODEL_NAME)
    
    checkpoint_dir = os.path.join("checkpoint", RUN_NAME)
    if not os.path.exists(checkpoint_dir):
        print("Error: No fine-tuned model found!")
        print("You must first train the model with: python3 ai.py")
        return None
    
    sess = gpt2.start_tf_sess()
    
    try:
        gpt2.load_gpt2(sess, model_name=MODEL_NAME, run_name=RUN_NAME)
        print("Model loaded successfully!")
        return sess
    except Exception as e:
        print(f"Error loading model: {e}")
        print("Try retraining with: python3 ai.py")
        return None

def generate_response(sess, prompt, length=300, temperature=0.8):
    try:
        print(f"\nGenerating response...")
        print("-" * 50)
        
        # Use a simpler format that matches the training data
        formatted_prompt = f"# Question: {prompt}\n# Answer:"
        
        generated = gpt2.generate(sess,
                                run_name=RUN_NAME,
                                length=length,
                                temperature=temperature,
                                top_k=50,
                                top_p=0.95,
                                prefix=formatted_prompt,
                                nsamples=1,
                                batch_size=1,
                                return_as_list=True,
                                truncate="<|endoftext|>")
        
        if generated and len(generated) > 0:
            response = generated[0]
            
            # Remove the formatted prompt from the beginning
            if response.startswith(formatted_prompt):
                response = response[len(formatted_prompt):].strip()
            
            # Clean up the response
            response = response.replace("\\n", "\n").strip()
            
            # Remove any repetitive patterns
            lines = response.split('\n')
            cleaned_lines = []
            for line in lines:
                if line.strip() and (not cleaned_lines or line.strip() != cleaned_lines[-1].strip()):
                    cleaned_lines.append(line)
                    # Stop if we hit a clear end marker or repetition
                    if len(cleaned_lines) > 1 and line.strip() in [l.strip() for l in cleaned_lines[:-1]]:
                        break
            
            response = '\n'.join(cleaned_lines).strip()
            
            # Ensure proper ending
            if response and not response.endswith(('.', '!', '?', ':')):
                response += "."
            
            return response if response else "I need more context to provide a helpful answer."
        else:
            return "Sorry, I couldn't generate an appropriate response. Could you rephrase your question?"
            
    except Exception as e:
        return f"An error occurred: {e}. Please try again!"

def interactive_mode(sess):
    print("\nLibreScript AI Interactive Mode")
    print("=" * 40)
    print("Type 'quit' or 'exit' to quit")
    print("Type 'help' to see example questions")
    print("Type 'settings' to modify parameters")
    print()
    
    length = 300
    temperature = 0.8
    
    while True:
        try:
            user_input = input("Your question: ").strip()
            
            if user_input.lower() in ['quit', 'exit', 'q']:
                print("Goodbye and see you soon!")
                break
                
            elif user_input.lower() == 'help':
                show_help()
                continue
                
            elif user_input.lower() == 'settings':
                length, temperature = configure_settings(length, temperature)
                continue
                
            elif not user_input:
                print("Please enter a question.")
                continue
            
            response = generate_response(sess, user_input, length, temperature)
            print(f"\n{response}\n")
            print("-" * 50)
            
        except KeyboardInterrupt:
            print("\nGoodbye and see you soon!")
            break
        except Exception as e:
            print(f"An error occurred: {e}")

def show_help():
    print("\nExample questions:")
    print("-" * 30)
    print("• How do I implement a function in Python?")
    print("• Can you explain async/await in JavaScript?")
    print("• I need help with a SQL query using JOIN")
    print("• How do I create a modern React component?")
    print("• Show me an efficient sorting algorithm in Java")
    print("• How do I properly handle exceptions in C#?")
    print("• I have a problem with database connections")
    print()

def configure_settings(current_length, current_temp):
    print(f"\nCurrent settings:")
    print(f"   Response length: {current_length} tokens")
    print(f"   Creativity: {current_temp}")
    print()
    
    try:
        length_input = input(f"New length (100-500, current: {current_length}): ").strip()
        if length_input:
            new_length = int(length_input)
            if 100 <= new_length <= 500:
                current_length = new_length
            else:
                print("Length must be between 100 and 500 tokens")
        
        temp_input = input(f"New creativity (0.3-1.0, current: {current_temp}): ").strip()
        if temp_input:
            new_temp = float(temp_input)
            if 0.3 <= new_temp <= 1.0:
                current_temp = new_temp
            else:
                print("Creativity must be between 0.3 and 1.0")
        
        print(f"Settings updated! Length: {current_length}, Creativity: {current_temp}")
        
    except ValueError:
        print("Invalid values, settings unchanged")
    
    return current_length, current_temp

def single_prompt_mode(sess, prompt):
    print(f"Question: {prompt}")
    response = generate_response(sess, prompt)
    print(f"\n{response}")

def main():
    print("LibreScript AI Assistant")
    print("=" * 35)
    
    if len(sys.argv) > 1:
        prompt = " ".join(sys.argv[1:])
        sess = load_model()
        if sess:
            single_prompt_mode(sess, prompt)
    else:
        sess = load_model()
        if sess:
            interactive_mode(sess)

if __name__ == "__main__":
    main() 