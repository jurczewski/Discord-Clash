using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DiscordClash.Core.Domain
{
    public class User : Entity
    {
        public ulong DiscordId { get; protected set; }
        public string DiscordNickname { get; protected set; }

        public User() { }

        public User(Guid id, ulong discordId, string discordNickname)
        {
            Id = id;
            SetDiscordData(discordId, discordNickname);

            CreatedAt = DateTime.Now;
            UpdatedAt = null;
        }

        public void SetDiscordData(ulong discordId, string discordNickname)
        {
            if (discordId == 0)
            {
                throw new ValidationException("User's discord Id cannot be 0.");
            }

            if (string.IsNullOrEmpty(discordNickname))
            {
                throw new ValidationException("User's discord Id cannot be empty.");
            }

            if (!Regex.IsMatch(discordNickname, "^.{3,32}#[0-9]{4}$"))
            {
                throw new ValidationException($"Discord nickname is invalid: {discordNickname}");
            }

            DiscordId = discordId;
            DiscordNickname = discordNickname;
            UpdatedAt = DateTime.Now;
        }
    }
}
