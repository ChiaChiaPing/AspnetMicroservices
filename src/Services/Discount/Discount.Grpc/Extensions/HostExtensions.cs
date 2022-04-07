    
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using Npgsql;
using System;

namespace Discount.Grpc.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            int? retryForAvailability = retry;

            const int MAX_RETRY_COUNT=  5; 
            var service = host.Services.GetService<IConfiguration>();
            var logger = host.Services.GetService<ILogger<TContext>>();

            var connStr = service.GetValue<string>("DatabaseSettings:ConnectionString");
            

            try
            {
                logger.LogInformation("Migrating the Postgresql Dattabase");

                // create connection and command obj
                using var conn = new NpgsqlConnection(connStr);
                conn.Open();
                using var command = new NpgsqlCommand()
                {
                    Connection = conn
                };


                // initialize the table
                command.CommandText = "DROP TABLE IF EXISTS COUPON";
                command.ExecuteNonQuery();

                command.CommandText = @"
                    CREATE TABLE COUPON (
                        Id Serial Not Null,
                        ProductName varchar(24) Primary Key,
                        Description text,
                        Amount int
                )";
                command.ExecuteNonQuery();


                // initialize the data
                command.CommandText = "Insert Into Coupon (ProductName, Description, Amount) values ( 'IphoneX', 'This is Ihpone', 100 )";
                command.ExecuteNonQuery();

                // initialize the data
                command.CommandText = "Insert Into Coupon (ProductName, Description, Amount) values ( 'HTC', 'This is HTC', 150 )";
                command.ExecuteNonQuery();

                //finish the process
                logger.LogInformation("Migrated Postgresql Database");

            }

            catch(NpgsqlException e)
            {
                logger.LogError(e, "The error occurred during migrating the postgresql database");
                if(retryForAvailability < MAX_RETRY_COUNT)
                {
                    retryForAvailability++;
                    Thread.Sleep(2000);
                    host.MigrateDatabase<TContext>(retryForAvailability);
                }
                else
                {
                  logger.LogError(e, "The error sttill occurred and over retry during migrating the postgresql database");
                  throw new NpgsqlException(e.Message);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "The unexpected error occurred during the migrating");
                throw new Exception(e.Message);
            }

            return host; // like builder pattern

        }
       
    }
}
