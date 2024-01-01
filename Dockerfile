# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 8080

# Accept the version build argument
ARG VERSION

# Base image for building the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["OperationStacked/OperationStacked.csproj", "OperationStacked/"]
RUN dotnet restore "OperationStacked/OperationStacked.csproj"
COPY . .
WORKDIR "/src/OperationStacked"
RUN dotnet build "OperationStacked.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "OperationStacked.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage/image
FROM base AS final
WORKDIR /app

# Set the version as an environment variable
ENV APP_VERSION=$VERSION

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OperationStacked.dll"]
