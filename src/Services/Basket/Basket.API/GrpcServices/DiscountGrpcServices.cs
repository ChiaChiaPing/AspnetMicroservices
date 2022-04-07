using System;
using System.Threading.Tasks;
using Discount.Grpc.Protos;


// like another DAO for handling thte communication with other grpc services
namespace Basket.API.GrpcServices
{
    public class DiscountGrpcServices
    {
        private readonly DiscountGrpcService.DiscountGrpcServiceClient _discountGrpcServiceClient;

        public DiscountGrpcServices(DiscountGrpcService.DiscountGrpcServiceClient discountGrpcServiceClient)
        {
            _discountGrpcServiceClient = discountGrpcServiceClient ?? throw new ArgumentNullException(nameof(discountGrpcServiceClient));
        }

        public async Task<CouponModel> GetDiscount(string name)
        {
            
            GetDiscountRequest request = new GetDiscountRequest() { ProductName = name };
            var result = await _discountGrpcServiceClient.GetDiscountAsync(request);
            return result;
        }

    }
}
