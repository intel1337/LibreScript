#!/usr/bin/env python3
"""
LibreScript AI API - Flask REST API for AI
Bridges the .NET backend with the AI model
"""

from flask import Flask, request, jsonify
from flask_cors import CORS
import gpt_2_simple as gpt2
import tensorflow as tf
import os
import threading
import time
import logging
from datetime import datetime


MODEL_NAME = "124M"
RUN_NAME = "librescript_code_model"
API_PORT = 5050
API_HOST = "0.0.0.0"

logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

app = Flask(__name__)
CORS(app)
tf_session = None
model_loaded = False
loading_lock = threading.Lock()

def load_ai_model():
    """Load the AI model at startup"""
    global tf_session, model_loaded
    
    with loading_lock:
        if model_loaded:
            return True
            
        try:
            logger.info("Loading AI model...")
            
            if not os.path.isdir(os.path.join("models", MODEL_NAME)):
                logger.info(f"Downloading base model {MODEL_NAME}...")
                gpt2.download_gpt2(model_name=MODEL_NAME)
            
            checkpoint_dir = os.path.join("checkpoint", RUN_NAME)
            if not os.path.exists(checkpoint_dir):
                logger.error("No fine-tuned model found!")
                return False
            
            tf_session = gpt2.start_tf_sess()
            
            gpt2.load_gpt2(tf_session, 
                          model_name=MODEL_NAME,
                          run_name=RUN_NAME)
            
            model_loaded = True
            logger.info("AI model loaded successfully!")
            return True
            
        except Exception as e:
            logger.error(f"Error loading model: {e}")
            return False

def generate_ai_response(prompt, length=200, temperature=0.7):
    """Generate a response with the AI"""
    global tf_session, model_loaded
    
    if not model_loaded:
        return {"error": "AI model not loaded"}
    
    try:
        logger.info(f"Generating response for: {prompt[:50]}...")
        
        generated = gpt2.generate(tf_session,
                                run_name=RUN_NAME,
                                length=length,
                                temperature=temperature,
                                prefix=prompt,
                                nsamples=1,
                                batch_size=1,
                                return_as_list=True)
        
        if generated and len(generated) > 0:
            response = generated[0]
            if response.startswith(prompt):
                response = response[len(prompt):].strip()
            
            logger.info("Response generated successfully")
            return {"response": response}
        else:
            return {"error": "No response generated"}
            
    except Exception as e:
        logger.error(f"Error during generation: {e}")
        return {"error": str(e)}

@app.route("/", methods=["GET"])
def health_check():
    """API health check"""
    return jsonify({
        "status": "ok",
        "service": "LibreScript AI API",
        "model_loaded": model_loaded,
        "timestamp": datetime.now().isoformat()
    })

@app.route("/status", methods=["GET"])
def get_status():
    """Get detailed API status"""
    checkpoint_dir = os.path.join("checkpoint", RUN_NAME)
    
    status = {
        "service": "LibreScript AI API",
        "model_loaded": model_loaded,
        "model_name": MODEL_NAME,
        "run_name": RUN_NAME,
        "checkpoint_exists": os.path.exists(checkpoint_dir),
        "timestamp": datetime.now().isoformat()
    }
    
    if os.path.exists(checkpoint_dir):
        try:
            counter_file = os.path.join(checkpoint_dir, "counter")
            if os.path.exists(counter_file):
                with open(counter_file, 'r') as f:
                    status["training_steps"] = int(f.read().strip())
        except:
            pass
    
    return jsonify(status)

@app.route("/generate", methods=["POST"])
def generate_response():
    """Generate a response with the AI"""
    try:
        if not model_loaded:
            return jsonify({
                "error": "AI model not available",
                "message": "The model is not yet loaded or training has not been performed"
            }), 503
        
        data = request.get_json()
        
        if not data:
            return jsonify({"error": "JSON data required"}), 400
        
        prompt = data.get("prompt", "").strip()
        if not prompt:
            return jsonify({"error": "The 'prompt' field is required"}), 400
        
        length = data.get("length", 200)
        temperature = data.get("temperature", 0.7)
        
        if not (50 <= length <= 500):
            return jsonify({"error": "Length must be between 50 and 500"}), 400
        
        if not (0.1 <= temperature <= 1.0):
            return jsonify({"error": "Temperature must be between 0.1 and 1.0"}), 400

        result = generate_ai_response(prompt, length, temperature)
        
        if "error" in result:
            return jsonify(result), 500
        
        return jsonify({
            "success": True,
            "prompt": prompt,
            "response": result["response"],
            "parameters": {
                "length": length,
                "temperature": temperature
            },
            "timestamp": datetime.now().isoformat()
        })
        
    except Exception as e:
        logger.error(f"Error in /generate: {e}")
        return jsonify({"error": "Internal server error"}), 500

@app.route("/reload", methods=["POST"])
def reload_model():
    """Reload the AI model"""
    global model_loaded
    
    try:
        model_loaded = False
        success = load_ai_model()
        
        if success:
            return jsonify({
                "success": True,
                "message": "Model reloaded successfully"
            })
        else:
            return jsonify({
                "success": False,
                "message": "Failed to reload model"
            }), 500
            
    except Exception as e:
        logger.error(f"Error during reload: {e}")
        return jsonify({"error": str(e)}), 500

@app.errorhandler(404)
def not_found(error):
    return jsonify({"error": "Endpoint not found"}), 404

@app.errorhandler(405)
def method_not_allowed(error):
    return jsonify({"error": "Method not allowed"}), 405

@app.errorhandler(500)
def internal_error(error):
    return jsonify({"error": "Internal server error"}), 500

def main():
    """Main function"""
    print("LibreScript AI API")
    print("=" * 25)
    print(f"Starting on http://{API_HOST}:{API_PORT}")
    
    threading.Thread(target=load_ai_model, daemon=True).start()
    
    app.run(
        host=API_HOST,
        port=API_PORT,
        debug=False,
        threaded=True
    )

if __name__ == "__main__":
    main() 