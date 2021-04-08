#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Apps/DiscordClash.Bot/DiscordClash.Bot.csproj", "src/Apps/DiscordClash.Bot/"]
RUN dotnet restore "src/Apps/DiscordClash.Bot/DiscordClash.Bot.csproj"
COPY . .
WORKDIR "/src/src/Apps/DiscordClash.Bot"
RUN dotnet build "DiscordClash.Bot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DiscordClash.Bot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DiscordClash.Bot.dll"]