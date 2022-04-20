using EventBus.Messages.Common;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Ordering.API.EventBusConsumer;
using Ordering.Application;
using Ordering.Infrastructure;

namespace Ordering.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddApplicationServices();
            services.AddInfrastructureServices(Configuration);

            services.AddControllers();


            services.AddSwaggerGen(c => {

                c.SwaggerDoc("v1", new OpenApiInfo() { Title = "Ordering API", Version = "v1" });

            });

            // MassTransit RabbitMQ Configuration. define the rabbitmq connection.
            services.AddMassTransit(config =>
            {

                // add a consumer
                config.AddConsumer<BasketCheckConsumer>();

                config.UsingRabbitMq((ctx, cfg) =>
                {
                    
                    cfg.Host(Configuration.GetValue<string>("EventBusSettings:HostAddress"));

                    // subscribe the queue
                    cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, cfg =>
                     {
                         // create a subscriber from the inherited consumer with context based on the added consumer.
                         cfg.ConfigureConsumer<BasketCheckConsumer>(ctx);
                     });
                });
            });

            // let MassTransit as host services
            services.AddMassTransitHostedService();
            services.AddAutoMapper(typeof(Program));

            // so that this class can be created in the aspnet project.
            services.AddScoped<BasketCheckConsumer>();


           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
