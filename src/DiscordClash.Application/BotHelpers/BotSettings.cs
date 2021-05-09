namespace DiscordClash.Application.BotHelpers
{
    public class BotSettings
    {
        /// <summary>
        /// The command prefix that this bot will respond to.
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// The Discord bot token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Discord Channel Id for event related notifications.
        /// </summary>
        public ulong EventsChannelId { get; set; }
    }
}
