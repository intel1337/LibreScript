FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app


COPY ../../backend-api/*.csproj ./
RUN dotnet restore
COPY ../../backend-api ./


RUN dotnet publish -c Release -o out

# Use the official .NET runtime image for running
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/out .


EXPOSE 5028

ENV ASPNETCORE_URLS=http://0.0.0.0:5028
ENV ASPNETCORE_ENVIRONMENT=Production

# Run the application
ENTRYPOINT ["dotnet", "MonApiBackend.dll"]

