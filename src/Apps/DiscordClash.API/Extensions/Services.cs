using DiscordClash.Core.Repositories;
using DiscordClash.Infrastructure.Repositories.InMemory;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordClash.API.Extensions
{
    public static class Services
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IEventRepository, InMemoryEventRepository>();

            return services;
        }
    }
}
