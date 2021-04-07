using System.Collections.Generic;
using DiscordClash.API.Framework.Options;

namespace DiscordClash.API.Framework
{
    public class LoggerOptions
    {
        public string MinimumLevel { get; set; } = "Information";
        public ConsoleOptions Console { get; set; }
        public FileOptions File { get; set; }
        public DataDogOptions DataDog { get; set; }
        public IDictionary<string, string> MinimumLevelOverrides { get; set; }
        public IEnumerable<string> ExcludePaths { get; set; }
        public IEnumerable<string> ExcludeProperties { get; set; }
        public IDictionary<string, object> Tags { get; set; }
    }
}
