using System;
using AutoMapper;
using Discount.Grpc.Protos;
using Discount.Grpc.Entities;

namespace Discount.Grpc.Mapper
{
    public class DiscountProfile: Profile
    {
        public DiscountProfile()
        {
            // create a mapper and can do two way mapper.
            CreateMap<Coupon, CouponModel>().ReverseMap();
        }
    }
}
