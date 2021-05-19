using DiscordClash.API.Extensions;
using DiscordClash.API.Middleware;
using DiscordClash.Infrastructure.Config;
using EasyNetQ;
using Figgle;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;

namespace DiscordClash.API
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.Configure<MongoDb>(Configuration.GetSection("mongoDb"));
            services.AddSingleton(RabbitHutch.CreateBus(Configuration["rabbitMq:connectionString"]));
            services.AddConfiguredSwagger()
                .AddServices()
                .AddInfrastructure(Configuration)
                .AddCustomHealthChecks(Configuration);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.ProcessReceivedMessages();

            DisplayBanner();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Generated swagger json and swagger ui middleware
            app.UseCustomSwagger();

            // Global cors policy
            app.UseCors(x => x
                .SetIsOriginAllowed(_ => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithExposedHeaders("Location"));

            // Global error handler
            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCustomEndpoints();
        }

        private static void DisplayBanner()
        {
            var name = Assembly.GetCallingAssembly().GetName().Name;
            Console.WriteLine(FiggleFonts.Doom.Render(name!));
        }
    }
}
