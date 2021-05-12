using MediatR;
using System;

namespace DiscordClash.Application.Requests
{
    public class NotifyAboutEvent : IRequest
    {
        public string FullName { get; set; }
        public DateTime StarTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public bool IsFree { get; set; }
        public Game GameCode { get; set; }
    }
}
