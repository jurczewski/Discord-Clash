using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordClash.Application.BotHelpers;
using DiscordClash.Application.Commands;
using DiscordClash.Application.Endpoints;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace DiscordClash.Bot.Handlers
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly BotSettings _settings;
        private readonly IServiceProvider _provider;
        private readonly ILogger<CommandHandler> _logger;
        private readonly IDiscordClashApi _api;

        public CommandHandler(DiscordSocketClient discord, CommandService commands, IOptions<BotSettings> settings, IServiceProvider provider, ILogger<CommandHandler> logger, IDiscordClashApi api)
        {
            _discord = discord;
            _commands = commands;
            _settings = settings.Value;
            _provider = provider;
            _logger = logger;
            _api = api;

            _discord.MessageReceived += OnMessageReceivedAsync;
            _discord.ReactionAdded += HandleReactionAddedAsync;
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

        private async Task HandleReactionAddedAsync(Cacheable<IUserMessage, ulong> cachedMessage, ISocketMessageChannel originChannel, SocketReaction reaction)
        {
            var message = await cachedMessage.GetOrDownloadAsync();
            if (message is null) return;

            var channel = (SocketGuildChannel)message.Channel;
            await channel.Guild.DownloadUsersAsync();

            if (message.Author.Id != _discord.CurrentUser.Id) return;

            if (reaction.User.IsSpecified)
            {
                var userValue = reaction.User.Value;
                var discordNickName = userValue.GetFullNickName();
                _logger.LogInformation("User {@discordNickName} (Id: '{@Id}) just added a reaction '{@Emote}' to {@Author}'s message ({@msgId}).", discordNickName, userValue.Id, reaction.Emote.Name, message.Author.GetFullNickName(), message.Id);

                var cmd = new SignUpToEvent
                {
                    User = new UserCmd
                    {
                        DiscordId = userValue.Id,
                        DiscordNickname = discordNickName
                    },
                    Choice = ParseEmoteToUint(reaction.Emote),
                    EventMsgId = message.Id
                };

                try
                {
                    await _api.SignUpToEvent(cmd);
                    _logger.LogInformation("Request {request} was sent to API {@cmd}", nameof(SignUpToEvent), cmd);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Unexpected error while sending a {@name} request. {@request}", nameof(SignUpToEvent), cmd);
                }
            }
            else
            {
                _logger.LogError("Cannot specify user identity. Somebody has just added a reaction '{@emote}' to message ({@id}).", reaction.Emote, message.Id);
            }
        }

        private static uint ParseEmoteToUint(IEmote emote)
        {
            if (emote.Name == Emojis.Tick.Name)
            {
                return 5;
            }

            if (emote.Name == Emojis.Cross.Name)
            {
                return 1;
            }

            return 3;
        }
    }
}
