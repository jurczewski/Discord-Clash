using Discord;
using Discord.WebSocket;
using DiscordClash.Application.BotHelpers;
using DiscordClash.Application.Messages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordClash.Application.UseCases
{
    public class NotifyAboutNewEventUseCase
    {
        private readonly DiscordSocketClient _client;
        private readonly ILogger<NotifyAboutNewEventUseCase> _logger;

        private const ulong EventsChannelId = 831225771967119362u; //todo: move to appsettings

        public NotifyAboutNewEventUseCase(DiscordSocketClient client, ILogger<NotifyAboutNewEventUseCase> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task Execute(NewEvent cmd)
        {
            var builder = new EmbedBuilder
            {
                Color = new Color(235, 64, 52),
                Description = $"🕹 {"New e-sport event!".ToBold()}"
                              + Environment.NewLine
                              + "React with a emote to sing up"
            };

            builder.AddEventToBuilder(cmd);

            var emojiCodes = new List<IEmote>
            {
                Emojis.Tick,
                Emojis.Cross
            };

            // Create the poll in the event channel.
            var channel = _client.GetChannel(EventsChannelId);
            await FakeTyping(channel);
            var sent = await ((IMessageChannel)channel).SendMessageAsync(string.Empty, false, builder.Build());

            await sent.AddReactionsAsync(emojiCodes.ToArray());
            _logger.LogInformation("New event with name: {@name} was posted to Discord channel. {@cmd}", cmd.FullName, cmd);
        }

        private static async Task FakeTyping(SocketChannel channel)
        {
            await ((IMessageChannel)channel).TriggerTypingAsync();
            Thread.Sleep(1000);
        }
    }
}
