# LibreScript AI Service

Artificial intelligence service for LibreScript using GPT-2 for code generation based on community questions and answers.

## Installation

### Prerequisites
- Python 3.7+
- pip3
- LibreScript backend running on `http://localhost:5028`

### Automatic installation
```bash
./train.sh
```

### Manual installation
```bash
pip3 install -r requirements.txt
```

## Usage

### Interactive script (recommended)
```bash
./train.sh
```

### Direct command line

#### Train from checkpoints (default)
```bash
python3 ai.py
```

#### Force new training from scratch
```bash
python3 ai.py --fresh
```

#### Generate code only (no training)
```bash
python3 ai.py --generate-only
```

#### Custom training with specific number of steps
```bash
python3 ai.py --steps 2000
```

#### Show checkpoint information
```bash
python3 ai.py --info
```

#### Prompt the AI (interactive mode)
```bash
python3 prompt.py
# or
./prompt.sh
```

#### Prompt the AI (single prompt)
```bash
python3 prompt.py "How to create a function in Python?"
./prompt.sh "JavaScript async/await example"
```

## File Structure

```
ai-service/
├── ai.py                   # Main training script
├── prompt.py              # Interactive prompt tool
├── train.sh               # Interactive training script
├── prompt.sh              # Launcher for prompt tool
├── requirements.txt       # Python dependencies
├── README.md             # This documentation
├── checkpoint/           # Model checkpoints (automatically created)
│   └── librescript_code_model/
├── models/               # GPT-2 models (automatically downloaded)
│   └── 124M/
└── librescript_dataset.txt  # Training dataset (automatically generated)
```

## How it works

1. **Data retrieval**: The script fetches all posts and comments from the LibreScript API
2. **Dataset preparation**: Data is formatted for GPT-2 training
3. **Training**: The model is fine-tuned on LibreScript data
4. **Generation**: The model can generate code based on prompts

## Configuration

You can modify parameters in `ai.py`:

```python
# LibreScript API Configuration
API_BASE_URL = "http://localhost:5028/api"

# Model Configuration   
MODEL_NAME = "124M"          # GPT-2 model size
DATA_FILE = "librescript_dataset.txt"
RUN_NAME = "librescript_code_model"
TRAINING_STEPS = 1000        # Number of training steps
```

## Checkpoint System

### How it works

The AI resumes fine-tuning from the **last saved checkpoint**. In your current case:

- **Current model**: `model-3` 
- **Current step**: `3`
- **Location**: `checkpoint/librescript_code_model/`

When you restart training, the AI will:
1. Load the `model-3` model
2. Resume from step 3
3. Continue training until the target step (default 1000)

### System advantages

- **Automatic resumption**: Training automatically resumes from the last checkpoint
- **No progress loss**: In case of interruption, work is not lost
- **Incremental training**: Ability to add more training steps
- **Time saving**: Avoids starting from scratch
- **Detailed tracking**: Use `python3 ai.py --info` to see current status

## Troubleshooting

### API connection error
- Check that the LibreScript backend is running
- Check the API URL in the configuration

### TensorFlow errors
- Make sure you have the correct TensorFlow version (2.8.0)
- On some systems, you might need `tensorflow-macos` instead of `tensorflow`

### Memory issues
- Reduce the number of training steps
- Use a smaller model (124M instead of 355M or 774M)

## Code Generation

The model generates code based on prompts similar to LibreScript questions:

```
# Question: How to implement a function in Python
# Content: I need help with React components
# Language: JavaScript
```

The model will generate an appropriate response based on patterns learned from the LibreScript community.

## Using the Prompt Tool

### Interactive mode

Launch the interactive prompt tool:
```bash
python3 prompt.py
# or
./prompt.sh
```

Once launched, you can:
- Type your questions directly
- Use `help` to see examples
- Use `settings` to adjust parameters
- Use `quit` or `exit` to quit

### Single prompt mode

For a quick question:
```bash
python3 prompt.py "How to implement quicksort in Python?"
```

### Available parameters

- **Length**: Number of tokens generated (100-500)
- **Creativity**: Response creativity (0.3-1.0)
  - Lower = more precise and repetitive
  - Higher = more creative and varied

### Effective prompt examples

```
How to create a REST API in Node.js?
SQL query example with JOIN
# Question: How to handle async operations in JavaScript?
# Content: I need help with Promise chains
Binary search algorithm in Java
``` 