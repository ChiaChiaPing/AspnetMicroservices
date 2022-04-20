using System;
using System.Collections.Generic;
using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;

namespace Ordering.Application.Features.Orders.Queries
{

    // MediatR request 
    public class GetOrdersListQuery : IRequest<List<OrdersVm>>
    {

        public string UserName { get; set; }

        public GetOrdersListQuery(string userName)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        }
    }
}
