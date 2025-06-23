#!/usr/bin/env python3
"""
Test rapide du modèle LibreScript AI
"""

import gpt_2_simple as gpt2
import tensorflow as tf
import os

MODEL_NAME = "124M"
RUN_NAME = "librescript_code_model"

def quick_test():
    """Test rapide du modèle"""
    print("🧪 Test rapide du modèle LibreScript AI")
    print("=" * 40)
    
    # Vérifier que le modèle de base existe
    if not os.path.isdir(os.path.join("models", MODEL_NAME)):
        print(f"📥 Téléchargement du modèle de base {MODEL_NAME}...")
        gpt2.download_gpt2(model_name=MODEL_NAME)
    
    # Vérifier que le modèle fine-tuné existe
    checkpoint_dir = os.path.join("checkpoint", RUN_NAME)
    if not os.path.exists(checkpoint_dir):
        print("❌ Erreur: Aucun modèle fine-tuné trouvé!")
        print("💡 Vous devez d'abord entraîner le modèle avec: python3 ai.py")
        return
    
    print("🔄 Chargement du modèle...")
    sess = gpt2.start_tf_sess()
    
    try:
        gpt2.load_gpt2(sess, 
                      model_name=MODEL_NAME,
                      run_name=RUN_NAME)
        print("✅ Modèle chargé avec succès!")
        
        # Tests avec différents prompts
        test_prompts = [
            "# Question: How to create a function in Python?",
            "# Content: I need help with JavaScript",
            "Comment faire une boucle en Python",
            "SQL query example"
        ]
        
        for i, prompt in enumerate(test_prompts, 1):
            print(f"\n🧪 Test {i}/4: {prompt}")
            print("-" * 50)
            
            try:
                generated = gpt2.generate(sess,
                                        run_name=RUN_NAME,
                                        length=100,
                                        temperature=0.8,
                                        prefix=prompt,
                                        nsamples=1,
                                        batch_size=1,
                                        return_as_list=True)
                
                if generated and len(generated) > 0:
                    response = generated[0]
                    if response.startswith(prompt):
                        response = response[len(prompt):].strip()
                    print(f"🤖 Réponse: {response[:200]}...")
                else:
                    print("❌ Aucune réponse générée")
                    
            except Exception as e:
                print(f"❌ Erreur: {e}")
        
        print(f"\n✅ Test terminé! Le modèle fonctionne.")
        print("💡 Utilisez 'python3 prompt.py' pour l'interface interactive")
        
    except Exception as e:
        print(f"❌ Erreur lors du chargement: {e}")

if __name__ == "__main__":
    quick_test() 