FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ARG VERSION

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["OperationStacked/OperationStacked.csproj","OperationStacked/"]
RUN dotnet restore "OperationStacked/OperationStacked.csproj"
COPY . .
WORKDIR "/src/OperationStacked"
RUN dotnet build "OperationStacked.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OperationStacked.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
ENV APP_VERSION=$VERSION
COPY --from=publish /app/publish .
CMD ["dotnet", "OperationStacked.dll"]
