using System;
using AutoMapper;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Grpc.Core;
using Discount.Grpc.Entities;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountGrpcService.DiscountGrpcServiceBase
    {
        private readonly ILogger<DiscountService> _logger;
        private readonly IDiscountRepository _discountRepsitory;
        private readonly IMapper _mapper;

        public DiscountService(ILogger<DiscountService> logger, IDiscountRepository discountRepository, IMapper mapper)
        {
            _logger = logger;
            _discountRepsitory = discountRepository;
            _mapper = mapper;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _discountRepsitory.GetCoupon(request.ProductName);
            if(coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"The Product is not found based on the {request.ProductName}"));
            }
            // convert the query data to CouponModel that defined in the return message via mapper
            var couponModel = _mapper.Map<CouponModel>(coupon);
            _logger.LogInformation($"Retrieveve success ProductName:{couponModel.ProductName}, Description: {coupon.Description}");

            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.CouponModel);

            await _discountRepsitory.CreateCoupon(coupon);

            _logger.LogInformation($"The Produce with Coupon is created with productName: {coupon.ProductName} with {coupon.Id}");

            return _mapper.Map<CouponModel>(coupon);

        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.CouponModel);

            await _discountRepsitory.UpdateCoupon(coupon);

            _logger.LogInformation($"The Produce with Coupon is updated with productName: {coupon.ProductName} with {coupon.Id}");

            return _mapper.Map<CouponModel>(coupon);
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
           
            var result = await _discountRepsitory.DeleteCoupon(request.ProductName);

            _logger.LogInformation($"The Produce with Coupon is deleted with productName: {request.ProductName}");

            var success = new DeleteDiscountResponse()
            {
                Success = result
            };

            return success;
        }

    }
}
