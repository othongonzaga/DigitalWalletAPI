# Dockerfile

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Configuração do Kestrel para escutar na porta 80
ENV ASPNETCORE_URLS=http://0.0.0.0:80

# Expor a porta 80 para o Docker
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["DigitalWalletAPI/DigitalWalletAPI.csproj", "DigitalWalletAPI/"]
RUN dotnet restore "DigitalWalletAPI/DigitalWalletAPI.csproj"
COPY . . 
WORKDIR "/src/DigitalWalletAPI"
RUN dotnet build "DigitalWalletAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DigitalWalletAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DigitalWalletAPI.dll"]
