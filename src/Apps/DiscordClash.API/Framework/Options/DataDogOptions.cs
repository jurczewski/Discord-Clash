namespace DiscordClash.API.Framework.Options
{
    public class DataDogOptions
    {
        public string ApiKey { get; set; }
        public string Source { get; set; }
        public string Service { get; set; }
        public string[] Tags { get; set; }
        public bool Enabled { get; set; }
    }
}
