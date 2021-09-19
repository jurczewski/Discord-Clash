using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Reflection;

namespace DiscordClash.API.Extensions
{
    public static class Swagger
    {
        public static IServiceCollection AddConfiguredSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "DiscordClash.API",
                        Description = "**Discord Clash** is a distributed application written in .NET. An event management system, with Discord bot to control the application and a recommendation system to help users to know about new events for them.",
                        Contact = new OpenApiContact
                        {
                            Name = "Bartosz Jurczewski",
                            Url = new Uri("https://github.com/jurczewski/"),
                        }
                    });
                c.CheckIfExistsAndIncludeXmlComments();
            });

            return services;
        }

        public static void UseCustomSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(o => o.SwaggerEndpoint("/swagger/v1/swagger.json", Assembly.GetEntryAssembly()?.GetName().Name));
        }

        private static void CheckIfExistsAndIncludeXmlComments(this SwaggerGenOptions c)
        {
            var basePath = AppContext.BaseDirectory;
            var assemblyName = Assembly.GetEntryAssembly()?.GetName().Name;
            var fileName = Path.GetFileName(assemblyName + ".xml");
            var path = Path.Combine(basePath, fileName);

            if (File.Exists(path))
            {
                c.IncludeXmlComments(path, true);
            }
        }
    }
}
