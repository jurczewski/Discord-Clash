using System;

namespace DiscordClash.Application.Commands
{
    public record CreateNewEvent //todo: move to useCase and use Mediator
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

    public enum Game
    {
        All = 0,
        LeagueOfLegends = 1,
        Dota2 = 2,
        CounterStrike = 3,
        ApexLegends = 4,
        Overwatch = 5
    }
}
