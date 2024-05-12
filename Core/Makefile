build:
	@echo "Building the project"
	dotnet build
start:
	@echo "Starting the project"
	dotnet run --project Core.csproj
publish:
	dotnet publish -c Release
publish-to-hub:
	dotnet publish -c Release -p:ContainerRegistry=docker.io -p:ContainerImageName=phamquangvinh/dotnet-webapi
dcu: # docker-compo	se up : services core
	cd ../ && docker compose up -d core
dcd: # docker-compose down : services core
	cd ../ && docker compose down core
dcr: # docker-compose restart : services core
	cd ../ && docker compose restart core
gw: # git docker workflow to push docker image to the repository based on the main branch
	@echo triggering github workflow to push docker image to container
	@echo ensure that you have the gh-cli installed and authenticated.
	gh workflow run dotnet-cicd -f push_to_docker=true