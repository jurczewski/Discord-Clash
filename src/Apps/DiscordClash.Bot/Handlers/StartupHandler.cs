using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordClash.Application.BotHelpers;
using Microsoft.Extensions.Options;

namespace DiscordClash.Bot.Handlers
{
    public class StartupHandler
    {
        private readonly IServiceProvider _provider;
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly BotSettings _settings;

        public StartupHandler(IServiceProvider provider, DiscordSocketClient discord, CommandService commands, IOptions<BotSettings> settings)
        {
            _provider = provider;
            _discord = discord;
            _commands = commands;
            _settings = settings.Value;
        }

        public async Task StartAsync()
        {
            var discordToken = _settings.Token;
            if (string.IsNullOrWhiteSpace(discordToken))
            {
                throw new Exception("Please enter your bot's token into the `appsettings.json` file found in the applications root directory.");
            }

            await _discord.LoginAsync(TokenType.Bot, discordToken);
            await _discord.StartAsync();

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
        }
    }
}
