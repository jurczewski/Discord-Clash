using Figgle;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace DiscordClash.API.Extensions
{
    public static class Services
    {
        public static void AddServices(this IServiceCollection services)
        {
            //services.AddTransient<IEventService, EventService>();

            DisplayBanner();
        }

        private static void DisplayBanner()
        {
            var name = Assembly.GetCallingAssembly().GetName().Name;
            Console.WriteLine(FiggleFonts.Doom.Render(name!));
        }
    }
}
