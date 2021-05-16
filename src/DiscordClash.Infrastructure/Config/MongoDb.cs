namespace DiscordClash.Infrastructure.Config
{
    public class MongoDb
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
        public bool UseDbInMemory { get; set; }
    }
}
