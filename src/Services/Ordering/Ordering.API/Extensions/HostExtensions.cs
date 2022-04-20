using System;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;

namespace Ordering.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host,
                                                       Action<TContext,IServiceProvider> seeder,
                                                       int? retry = 0)
            where TContext: DbContext
        {

            var retryForAvailability = retry.Value;

            using (var scope = host.Services.CreateScope())
            {
                
                // host -> service scope -> service provider (services)-> get service via multiple method.
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();

                try
                {

                    logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);
                    /*
                    var retry = Policy.Handle<SqlException>()
                          .WaitAndRetry(
                              retryCount: retryForAvailability,
                              sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // 2,4,8,16,32 sc
                              onRetry: (exception, retryCount, context) =>
                              {
                                  logger.LogError($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}.");
                              });

                    //if the sql server container is not created on run docker compose this
                    //migration can't fail for network related exception. The retry options for DbContext only 
                    //apply to transient exceptions                    
                    retry.Execute(() => InvokeSeeder(seeder, context, services));
                    */

                    InvokeSeeder(seeder, context, services);



                    logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);

                }
                catch (SqlException ex)
                {

                    logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);

                    if(retryForAvailability < 50)
                    {
                        retryForAvailability += 1;
                        System.Threading.Thread.Sleep(2000);
                        MigrateDatabase<TContext>(host, seeder, retryForAvailability);
                    }

                }

            }

            return host;
        }

        private static void InvokeSeeder<TContext>(Action<TContext,IServiceProvider> seeder, TContext context, IServiceProvider services)
            where TContext: DbContext
        {
            // will invoke the OrderContext's Up method that generatetd from ef core
            context.Database.Migrate();

            // add on pre-configure data.
            seeder(context, services);
        }
    }
}
