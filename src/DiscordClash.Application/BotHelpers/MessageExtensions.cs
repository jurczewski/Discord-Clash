using Discord;
using DiscordClash.Application.Messages;

namespace DiscordClash.Application.BotHelpers
{
    public static class MessageExtensions
    {
        public static string ToBold(this string text)
        {
            return $"**{text}**";
        }

        public static void AddEventToBuilder(this EmbedBuilder builder, NewEvent cmd)
        {
            builder.AddField(f =>
            {
                f.Name = "Name";
                f.Value = cmd.FullName;
                f.IsInline = true;
            }).AddField(f =>
            {
                f.Name = "Star Time";
                f.Value = cmd.StarTime.Date.ToShortDateString();
            }).AddField(f =>
            {
                f.Name = "End Time";
                f.Value = cmd.EndTime.Date.ToShortDateString();
            }).AddField(f =>
            {
                f.Name = "Country";
                f.Value = cmd.Country;
            }).AddField(f =>
            {
                f.Name = "City";
                f.Value = cmd.City;
            }).AddField(f =>
            {
                f.Name = "Free?";
                f.Value = cmd.IsFree.ToString();
            }).AddField(f =>
            {
                f.Name = "GameCode";
                f.Value = cmd.GameCode;
            });
        }

        public static string GetFullNickName(this IUser user)
        {
            return $"{user.Username}#{user.Discriminator}";
        }
    }
}
