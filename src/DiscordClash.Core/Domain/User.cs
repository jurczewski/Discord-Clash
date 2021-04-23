using System;
using System.ComponentModel.DataAnnotations;

namespace DiscordClash.Core.Domain
{
    public class User
    {
        public Guid Id { get; protected set; }
        public string DiscordId { get; protected set; }
        public string DiscordNickname { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }

        public User(Guid id, string discordId, string discordNickname)
        {
            Id = id;
            SetDiscordData(discordId, discordNickname);

            CreatedAt = DateTime.Now;
            UpdatedAt = null;
        }

        public void SetDiscordData(string discordId, string discordNickname)
        {
            if (string.IsNullOrEmpty(discordId))
            {
                throw new ValidationException("User's discord Id cannot be empty.");
            }

            if (string.IsNullOrEmpty(discordNickname))
            {
                throw new ValidationException("User's discord Id cannot be empty.");
            }

            DiscordId = discordId;
            DiscordNickname = discordNickname;
            UpdatedAt = DateTime.Now;
        }
    }
}
