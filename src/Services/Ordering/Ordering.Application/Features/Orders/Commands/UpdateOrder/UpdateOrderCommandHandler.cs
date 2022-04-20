using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using System.Linq;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler: IRequestHandler<UpdateOrderCommand>
    {
        private readonly ILogger<UpdateOrderCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;

        public UpdateOrderCommandHandler(ILogger<UpdateOrderCommandHandler> logger, IMapper mapper, IOrderRepository orderRepository)
        {

            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
               
               
        }

        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderObj = await _orderRepository.GetByIdAsync(request.Id);
            if (orderObj == null)
            {
                
                throw new NotFoundException(nameof(Order), request.Id);
            }
            
            // map the existing object. like map and update the props of existing objec
            // not like just map and create a mapped object.
            _mapper.Map(request, orderObj, typeof(UpdateOrderCommand), typeof(Order));

            await _orderRepository.UpdateAsync(orderObj);

            _logger.LogInformation($"The order is updated with {orderObj.Id}");

            return Unit.Value; // if no return value defined in the hanndler, then return unit like representating command is a work unit.

        }
    }
}
