# Етап 1: Збірка проєкту (SDK)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копіюємо файли проєктів для відновлення залежностей (це пришвидшує збірку)
COPY ["BankSystem.Api/BankSystem.Api.csproj", "BankSystem.Api/"]
COPY ["BankSystem.Application/BankSystem.Application.csproj", "BankSystem.Application/"]
COPY ["BankSystem.Domain/BankSystem.Domain.csproj", "BankSystem.Domain/"]
COPY ["BankSystem.Infrastructure/BankSystem.Infrastructure.csproj", "BankSystem.Infrastructure/"]
RUN dotnet restore "BankSystem.Api/BankSystem.Api.csproj"

# Копіюємо всі інші вихідні коди та білдимо проєкт
COPY . .
WORKDIR "/src/BankSystem.Api"
RUN dotnet build "BankSystem.Api.csproj" -c Release -o /app/build

# Етап 2: Публікація готового бінарника
FROM build AS publish
RUN dotnet publish "BankSystem.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Етап 3: Запуск фінального контейнера (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BankSystem.Api.dll"]
