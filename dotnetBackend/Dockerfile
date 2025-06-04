FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY dotnetBackend/*.csproj ./dotnetBackend/
RUN dotnet restore ./dotnetBackend/MonApiBackend.csproj

COPY dotnetBackend/. ./dotnetBackend/
WORKDIR /app/dotnetBackend
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/dotnetBackend/out .
EXPOSE 5028


ENTRYPOINT ["dotnet run"]
