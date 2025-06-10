# syntax=docker/dockerfile:1

ARG DOTNET_VERSION=9.0

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS builder
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY --link MonApiBackend.csproj ./
RUN --mount=type=cache,target=/root/.nuget/packages \
    --mount=type=cache,target=/root/.cache/msbuild \
    dotnet restore

# Copy the rest of the source code
COPY --link . .

# Publish the application
RUN dotnet publish -c Release -o /app/publish --no-restore

# Final image
FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS final
WORKDIR /app

# Create a non-root user
RUN addgroup --system appgroup && adduser --system --ingroup appgroup appuser
USER appuser

COPY --from=builder /app/publish .

# Expose default ASP.NET port
EXPOSE 80

ENTRYPOINT ["dotnet", "MonApiBackend.dll"]
