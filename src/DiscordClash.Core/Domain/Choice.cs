using System;
using System.ComponentModel.DataAnnotations;

namespace DiscordClash.Core.Domain
{
    public class Choice : Entity
    {
        public Guid EventId { get; protected set; }
        public Guid UserId { get; protected set; }
        public uint Label { get; protected set; }

        public void SetUserId(Guid id)
        {
            if (id == Guid.Empty) throw new ValidationException("User id cannot be empty guid.");

            UserId = id;
        }
    }
}
