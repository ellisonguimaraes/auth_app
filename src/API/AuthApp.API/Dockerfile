FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["API/AuthApp.API/AuthApp.API.csproj", "API/AuthApp.API/"]
COPY ["Infra/AuthApp.Infra.CrossCutting.IoC/AuthApp.Infra.CrossCutting.IoC.csproj", "Infra/AuthApp.Infra.CrossCutting.IoC/"]
COPY ["Application/AuthApp.Application/AuthApp.Application.csproj", "Application/AuthApp.Application/"]
COPY ["Domain/AuthApp.Domain/AuthApp.Domain.csproj", "Domain/AuthApp.Domain/"]
COPY ["Infra/AuthApp.Infra.CrossCutting.Resources/AuthApp.Infra.CrossCutting.Resources.csproj", "Infra/AuthApp.Infra.CrossCutting.Resources/"]
COPY ["Infra/AuthApp.Infra.Data/AuthApp.Infra.Data.csproj", "Infra/AuthApp.Infra.Data/"]
COPY ["Services/AuthApp.Services/AuthApp.Services.csproj", "Services/AuthApp.Services/"]
RUN dotnet restore "API/AuthApp.API/AuthApp.API.csproj"
COPY . .
WORKDIR "/src/API/AuthApp.API"
RUN dotnet build "AuthApp.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AuthApp.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthApp.API.dll"]
