using DiscordClash.Core.Domain;

namespace DiscordClash.Application.Messages
{
    /// <summary>
    /// Defines queues names by template: domainObject.MessageObject.
    /// </summary>
    public static class Queues
    {
        public static string NewEvent => $"{nameof(Event)}.{nameof(Messages.NewEvent)}";
    }
}
