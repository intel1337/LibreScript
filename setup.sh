echo "Configuring Librescript CLI..."
echo "alias librescript='$(pwd)/LibreScriptBootstrap.sh'" >> ~/.zshrc
source ~/.zshrc

echo "alias librescript='$(pwd)/LibreScriptBootstrap.sh'" >> ~/.bashrc
source ~/.bashrc
sleep 2

echo "Librescript CLI configured successfully"
sleep 1
echo "Installing dependencies..."
cd frontend-web
npm install
cd ..

cd backend-api
dotnet restore
cd ..

cd ..

echo "Dependencies installed successfully"
sleep 1
echo ""
echo "Useful Commands :"
echo "--------------------------------"
echo "librescript --clean : Clean the environment"
echo "librescript --log : Start the environment with Kubernetes and Docker for Prod & Log the environment"
echo "librescript : Start the environment with Kubernetes and Docker for Prod"

echo "Github Repo : https://github.com/intel1337/LibreScript"




