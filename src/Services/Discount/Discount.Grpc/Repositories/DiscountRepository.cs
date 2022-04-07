using System;
using Dapper;
using System.Threading.Tasks;
using Discount.Grpc.Entities;
using Discount.Grpc.Data;

namespace Discount.Grpc.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private ICouponContext _conn;

        public DiscountRepository(ICouponContext conn)
        {
            _conn = conn;
        }

        public async Task<Coupon> GetCoupon(string productName)
        {
            using var conn = _conn.NpgsqlConnection;
            var coupon = await conn.QueryFirstOrDefaultAsync<Coupon>("" +
                "Select Id, ProductName, Description, Amount From Coupon where ProductName=@ProductName "
                , new { ProductName = productName });

            if (coupon == null)
                return new Coupon() { Id = 0, ProductName = "No Product", Description = "No Descripiottn", Amount = 0 };
            return coupon;

        }
        public async Task<bool> CreateCoupon(Coupon coupon)
        {
            using var conn = _conn.NpgsqlConnection;
            var affectedRow = await conn.ExecuteAsync("" +
                "Insert INTO Coupon(ProductName,Description,Amount) values (@ProductName, @Description, @Amount)"
                , new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

            return affectedRow != 0;
        }
        public async Task<bool> UpdateCoupon(Coupon coupon)
        {
            using var conn = _conn.NpgsqlConnection;
            var affectedRow = await conn.ExecuteAsync("" +
                "Update Coupon SET ProductName = @ProductName, Description=@Description, Amount=@Amount Where Id = @Id"
                , new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id });

            return affectedRow != 0;
        }
        public async Task<bool> DeleteCoupon(string productName)
        {
            using var conn = _conn.NpgsqlConnection;
            var affectedRow = await conn.ExecuteAsync("" +
                "Delete from Coupon where ProductName=@ProductName"
                , new { ProductName = productName });

            return affectedRow != 0;
        }
    }

}
