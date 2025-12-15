FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar arquivos de projeto
COPY ["src/HomeSpends.API/HomeSpends.API.csproj", "src/HomeSpends.API/"]
COPY ["src/HomeSpends.Application/HomeSpends.Application.csproj", "src/HomeSpends.Application/"]
COPY ["src/HomeSpends.Domain/HomeSpends.Domain.csproj", "src/HomeSpends.Domain/"]
COPY ["src/HomeSpends.Infrastructure/HomeSpends.Infrastructure.csproj", "src/HomeSpends.Infrastructure/"]

# Restaurar dependências
RUN dotnet restore "src/HomeSpends.API/HomeSpends.API.csproj"

# Copiar todo o código
COPY . .

# Build
WORKDIR /app/src/HomeSpends.API
RUN dotnet build "HomeSpends.API.csproj" -c Release -o /app/build

# Publicar
FROM build AS publish
RUN dotnet publish "HomeSpends.API.csproj" -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "HomeSpends.API.dll"]

