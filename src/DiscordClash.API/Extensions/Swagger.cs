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
        public static void AddConfiguredSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "DiscordClash.API",
                        Description = "",
                    });
                c.CheckIfExistsAndIncludeXmlComments();
            });
        }

        public static void UseCustomSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(o => o.SwaggerEndpoint("/swagger/v1/swagger.json", "Discord Clash"));
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
