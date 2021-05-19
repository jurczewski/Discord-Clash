using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordClash.Application.BotHelpers;
using DiscordClash.Application.Messages;
using EasyNetQ;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DiscordClash.Application.UseCases.Bot
{
    public class NotifyAboutNewEventUseCase
    {
        private readonly DiscordSocketClient _client;
        private readonly ILogger<NotifyAboutNewEventUseCase> _logger;
        private readonly ulong _eventsChannelId;
        private readonly IBus _bus;

        public NotifyAboutNewEventUseCase(DiscordSocketClient client, ILogger<NotifyAboutNewEventUseCase> logger, IOptions<BotSettings> settings, IBus bus)
        {
            _client = client;
            _logger = logger;
            _bus = bus;
            _eventsChannelId = settings.Value.EventsChannelId;
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

            var sent = await SendNotificationToChannel(builder, emojiCodes, cmd);

            await SendMsgIdWithEvent(sent, cmd);
        }

        private async Task<IUserMessage> SendNotificationToChannel(EmbedBuilder builder, List<IEmote> emojiCodes, NewEvent cmd)
        {
            var channel = _client.GetChannel(_eventsChannelId);
            await FakeTyping(channel);
            var sent = await ((IMessageChannel)channel).SendMessageAsync(string.Empty, false, builder.Build());
            await sent.AddReactionsAsync(emojiCodes.ToArray());
            _logger.LogInformation("New event with name: {@name} was posted to Discord channel. {@cmd}", cmd.FullName, cmd);

            return sent;
        }

        private async Task SendMsgIdWithEvent(IUserMessage sent, NewEvent cmd)
        {
            var msg = new PairDiscordMsgWithEvent { DiscordMsgId = sent.Id, EventId = cmd.Id };
            await _bus.SendReceive.SendAsync(Queues.PairMsgWithEvent, msg);
            _logger.LogInformation("Discord message Id was sent to API. {@msg}", msg);
        }

        private static async Task FakeTyping(SocketChannel channel)
        {
            await ((IMessageChannel)channel).TriggerTypingAsync();
            Thread.Sleep(1000);
        }
    }
}
