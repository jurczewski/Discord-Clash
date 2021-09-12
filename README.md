# Discord Clash [![Build](https://github.com/jurczewski/Discord-Clash/actions/workflows/build.yml/badge.svg)](https://github.com/jurczewski/Discord-Clash/actions/workflows/build.yml)

**Discord Clash** is a distributed application written in .NET. An event management system, with Discord bot to control the app and a recommendation system to help users know about new events for them.

## Authors

- [@jurczewski](https://github.com/jurczewski)

## Tech Stack

- .NET 5.0 with in C#
- [RabbitMQ](https://www.rabbitmq.com/) with [EasyNetQ](https://github.com/EasyNetQ/EasyNetQ) library
- MongoDB - for storing users and events
- [Discord.Net](https://github.com/discord-net/Discord.Net) - .NET API Wrapper for the Discord client
- [Cocona](https://github.com/mayuki/Cocona) - micro-framework for .NET Core console application

Other used libs: [Figgle](https://github.com/drewnoakes/figgle), [Refit](https://github.com/reactiveui/refit), [Serilog](https://github.com/serilog/serilog), [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) (Swagger)

## Architecture

![Components diagram](diagrams/export/Discord%20Clash%20-%20Components%20Diagram.png)

## Run Locally

Clone the project

```bash
  git clone https://github.com/jurczewski/Discord-Clash
```

Go to the project directory and start infrastructure (MongoDB and RabbitMQ)

```bash
  cd Discord-Clash
  docker-compose up
```

Go to API directory and run

```bash
  cd src/Apps/DiscordClash.API
  dotnet run
```

To see available endpoints go to `localhost:5001/swagger` or use files inside [`request`](requests) directory. To use them install a [VS Code Rest Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client).

### Bot

Before running a bot you need to enter your bot token and channel id for notifications in [`appsettings.json`](src/Apps/DiscordClash.Bot/appsettings.json).

After that, go to Bot directory and run:

```bash
  cd src/Apps/DiscordClash.Bot
  dotnet run
```
