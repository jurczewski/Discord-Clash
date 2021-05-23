using DiscordClash.Application.UseCases.API;
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
            services.AddTransient<PairDiscordMsgWithEventUseCase>();
            services.AddTransient<SignUpToEventUseCase>();
            services.AddTransient<GetAllEventsUseCase>();

            services.AddTransient<MessageHandler>();

            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var inMemory = configuration.GetSection("mongoDb").Get<MongoDb>().UseDbInMemory;
            if (inMemory)
            {
                services.AddTransient<IGenericRepository<Event>, InMemoryGenericRepository<Event>>();
                services.AddTransient<IGenericRepository<Choice>, InMemoryGenericRepository<Choice>>();
                services.AddTransient<IGenericRepository<User>, InMemoryGenericRepository<User>>();
                services.AddTransient<IUserRepository, InMemoryUserRepository>();
                services.AddTransient<IEventRepository, InMemoryEventRepository>();
            }
            else
            {
                services.AddTransient<IGenericRepository<Event>, MongoGenericRepository<Event, EventDb>>();
                services.AddTransient<IGenericRepository<Choice>, MongoGenericRepository<Choice, ChoiceDb>>();
                services.AddTransient<IGenericRepository<User>, MongoGenericRepository<User, UserDb>>();
                services.AddTransient<IUserRepository, MongoUserRepository>();
                services.AddTransient<IEventRepository, MongoEventRepository>();
            }

            return services;
        }

        /// <summary>
        /// Process all received messages from RabbitMQ
        /// </summary>
        /// <param name="services"></param>
        public static void ProcessReceivedMessages(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var msgService = provider.GetService<MessageHandler>();
            msgService?.ProcessMessages();
        }
    }
}
