using System;
using AutoMapper;
using EventBus.Messages;
using Ordering.Application.Features.Orders.Commands.CheckOutOrder;

namespace Ordering.API.Mapper
{
    public class OrderProfile: Profile
    {
        public OrderProfile()
        {
            CreateMap<CheckOutOrderCommand,BasketCheckoutEvent>().ReverseMap();
        }
    }
}
