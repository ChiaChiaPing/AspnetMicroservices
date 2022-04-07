using System;
using Npgsql;

namespace Discount.Grpc.Data
{
    public interface ICouponContext
    {
        public NpgsqlConnection NpgsqlConnection { get; }
    }
}
