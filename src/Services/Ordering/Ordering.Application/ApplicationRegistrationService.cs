using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using MediatR;
using FluentValidation;
using Ordering.Application.Behaviors;

namespace Ordering.Application
{
    public static class ApplicationRegistrationService
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            // will find out which class implement the Profile, tthen will register that class to the IOC Container
            // register the service that DI to ourselves
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // register that service implemnet IValidator, not direct implement can be indirectly implement via parent class./
            // ex. AbstractorValidator
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // register the service that DI to other class
            // register the service that implement IRequest and IRequ
            services.AddMediatR(Assembly.GetExecutingAssembly());

            //register the ppieline behavior that DI to others
            services.AddTransient(typeof(IPipelineBehavior<,>),typeof(ValidationBehaviour<,>));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnHandledBehaviour<,>));


            return services;
        }
    }
}
