﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;

namespace Ordering.Application.Features.Orders.Queries
{
    //MediatR define the Handler
    public class GetOrdersListQueryHandler: IRequestHandler<GetOrdersListQuery, List<OrdersVm>>
    {

        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;

        public GetOrdersListQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        public async Task<List<OrdersVm>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
        {
            var ordersList = await _orderRepository.GetOrderByUserName(request.UserName);
            return _mapper.Map<List<OrdersVm>>(ordersList);

        }
    }
}
