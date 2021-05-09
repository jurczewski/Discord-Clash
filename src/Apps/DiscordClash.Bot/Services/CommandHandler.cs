using Discord.Commands;
using Discord.WebSocket;
using DiscordClash.Application.BotHelpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace DiscordClash.Bot.Services
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly BotSettings _settings;
        private readonly IServiceProvider _provider;
        private readonly ILogger<CommandHandler> _logger;

        public CommandHandler(DiscordSocketClient discord, CommandService commands, IOptions<BotSettings> settings, IServiceProvider provider, ILogger<CommandHandler> logger)
        {
            _discord = discord;
            _commands = commands;
            _settings = settings.Value;
            _provider = provider;
            _logger = logger;

            _discord.MessageReceived += OnMessageReceivedAsync;
        }

        private async Task OnMessageReceivedAsync(SocketMessage s)
        {
            // Ensure the message is from a user/bot
            if (s is not SocketUserMessage msg) return;
            if (msg.Author.Id == _discord.CurrentUser.Id) return; // Ignore self when checking commands

            var context = new SocketCommandContext(_discord, msg); // Create the command context

            var argPos = 0; // Check if the message has a valid command prefix
            if (msg.HasStringPrefix(_settings.Prefix, ref argPos) ||
                msg.HasMentionPrefix(_discord.CurrentUser, ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _provider); // Execute the command

                if (!result.IsSuccess)
                {
                    // If not successful, reply with the error.
                    await context.Channel.SendMessageAsync(result.ToString());
                    _logger.LogInformation("Unknown command.");
                }
            }
        }
    }
}
