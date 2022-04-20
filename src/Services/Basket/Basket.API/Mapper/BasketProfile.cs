using System;
using AutoMapper;
using Basket.API.Entities;
using EventBus.Messages;

namespace Basket.API.Mapper
{
    public class BasketProfile: Profile
    {
        public BasketProfile()
        {
            CreateMap<BasketCheckout, BasketCheckoutEvent>().ReverseMap();
        }
    }
}
