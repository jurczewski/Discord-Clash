using DiscordClash.API.Extensions;
using EasyNetQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.AddConfiguredSwagger();
            services.AddHealthChecks();
            services.AddServices();

            services.AddSingleton(RabbitHutch.CreateBus(Configuration["rabbitMq:connectionString"]));

            // todo: add rabbitmq healthcheck
            //services.AddHealthChecks()
            //    .AddRabbitMQ(rabbitConnectionString: rabbitMq);
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

            app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCustomEndpoints();
        }
    }
}
