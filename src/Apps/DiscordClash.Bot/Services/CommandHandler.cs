using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace DiscordClash.Bot.Services
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly IConfigurationRoot _config;
        private readonly IServiceProvider _provider;

        public CommandHandler(DiscordSocketClient discord, CommandService commands, IConfigurationRoot config,
            IServiceProvider provider)
        {
            _discord = discord;
            _commands = commands;
            _config = config;
            _provider = provider;

            _discord.MessageReceived += OnMessageReceivedAsync;
        }

        private async Task OnMessageReceivedAsync(SocketMessage s)
        {
            // Ensure the message is from a user/bot
            if (s is not SocketUserMessage msg) return;
            if (msg.Author.Id == _discord.CurrentUser.Id) return; // Ignore self when checking commands

            var context = new SocketCommandContext(_discord, msg); // Create the command context

            var argPos = 0; // Check if the message has a valid command prefix
            if (msg.HasStringPrefix(_config["prefix"], ref argPos) ||
                msg.HasMentionPrefix(_discord.CurrentUser, ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _provider); // Execute the command

                if (!result.IsSuccess) // If not successful, reply with the error.
                    await context.Channel.SendMessageAsync(result.ToString());
            }
        }
    }
}
