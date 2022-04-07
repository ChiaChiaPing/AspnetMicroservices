using System;
using Npgsql;
using Microsoft.Extensions.Configuration;

namespace Discount.Grpc.Data
{
    public class CouponContext : ICouponContext
    {
        // only applicable for outside that can only get. this is not restricted inner access (ex. within same calss)
        public NpgsqlConnection NpgsqlConnection { get; }
        private IConfiguration _configuration;

        public CouponContext(IConfiguration configuration)
        {
            _configuration = configuration;
            var connString = _configuration.GetValue<string>("DatabaseSettings:ConnectionString");
            NpgsqlConnection = new NpgsqlConnection(connString);
        }

    }
}
