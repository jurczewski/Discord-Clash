using DiscordClash.Application.UseCases;
using DiscordClash.Core.Domain;
using DiscordClash.Core.Repositories;
using DiscordClash.Infrastructure.Config;
using DiscordClash.Infrastructure.Dto;
using DiscordClash.Infrastructure.Repositories;
using DiscordClash.Infrastructure.Repositories.InMemory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordClash.API.Extensions
{
    public static class Services
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<CreateNewEventUseCase>();
            services.AddTransient<RemoveEventUseCase>();

            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var inMemory = configuration.GetSection("mongoDb").Get<MongoDb>().UseDbInMemory;
            if (inMemory)
            {
                services.AddTransient<IGenericRepository<Event>, InMemoryEventRepository>();
            }
            else
            {
                services.AddTransient<IGenericRepository<Event>, MongoGenericRepository<EventDb, Event>>();
            }

            return services;
        }
    }
}
