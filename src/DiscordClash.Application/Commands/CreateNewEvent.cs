using System;

namespace DiscordClash.Application.Commands
{
    public record CreateNewEvent : ICommand
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime StarTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public bool IsFree { get; set; }
        public Game GameCode { get; set; }
    }
}
