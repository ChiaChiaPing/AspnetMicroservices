using System;
using Npgsql;

namespace Discount.API.Data
{
    public interface ICouponContext
    {
        public NpgsqlConnection NpgsqlConnection { get; }
    }
}
