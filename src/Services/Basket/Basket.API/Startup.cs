using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Basket.API.Repositories;
using Microsoft.OpenApi.Models;
using Discount.Grpc.Protos;
using Basket.API.GrpcServices;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Grpc.Core;
using MassTransit;

namespace Basket.API
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
            // Redis Configuration
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetValue<string>("CacheSetting:ConnectionString");
            }); 

            // General Configuration
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddAutoMapper(typeof(Startup));

            // Grpc configuration - register grpc client
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            services.AddGrpcClient<DiscountGrpcService.DiscountGrpcServiceClient>(o =>
            {
                o.Address = new Uri(Configuration["GrpcSettings:DiscountUri"]);
                //o.ChannelOptionsActions.Add(channelOptions => channelOptions.Credentials = ChannelCredentials.Insecure);

            });

            // register the  customized  grpc DAO
            services.AddScoped<DiscountGrpcServices>();

            // MassTransit RabbitMQ Configuration. define the rabbitmq connection.
            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(Configuration.GetValue<string>("EventBusSettings:HostAddress"));
                }); 
            });

            // let MassTransit as host services
            services.AddMassTransitHostedService();

            services.AddControllers();
            services.AddSwaggerGen(c => {

                c.SwaggerDoc("v1", new OpenApiInfo() {Title = "Basket API", Version = "v1" });

            });
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
