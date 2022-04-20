using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Infrastructure.Persistence;
using Ordering.Infrastructure.Repositories;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Infrastructure.Email;
using Microsoft.EntityFrameworkCore;

namespace Ordering.Infrastructure
{
    public static class InfrastructureRegistrationServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {

            // register the DB Context
            services.AddDbContext<OrderContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("OrderingConnectionString"));

            });


            // register for Mediator Purpose.
            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));

            services.AddScoped<IOrderRepository, OrderRepository>();


            // configure the props of email settings obj using app settings.
            // when new EmailSettings object, can use variables in app settings as the prop of the object.
            services.Configure<EmailSettings>(options => {

                configuration.GetSection("EmailSettings");

            });

            services.AddTransient<IEmailService, EmailService>();


            return services;

        }
    }
}
