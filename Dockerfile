# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Kopiujemy tylko plik projektu i przywracamy paczki
COPY WebApplication1/WebApplication1.csproj ./
RUN dotnet restore

# Kopiujemy resztę kodu
COPY WebApplication1/. ./

# Publikujemy do czystego folderu /app
RUN dotnet publish -c Release -o /app

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Kopiujemy opublikowane pliki z build stage
COPY --from=build /app ./

# Uruchamiamy aplikację
ENTRYPOINT ["dotnet", "WebApplication1.dll"]
