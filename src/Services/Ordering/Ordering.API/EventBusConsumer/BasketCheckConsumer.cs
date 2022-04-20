using System;
using System.Threading.Tasks;
using AutoMapper;
using EventBus.Messages;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Features.Orders.Commands.CheckOutOrder;

namespace Ordering.API.EventBusConsumer
{
    public class BasketCheckConsumer: IConsumer<BasketCheckoutEvent>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<BasketCheckConsumer> _logger;
        private readonly IMediator _mediator;


        public BasketCheckConsumer(IMapper mapper, ILogger<BasketCheckConsumer> logger, IMediator mediator )
        {
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }


        // worker based trigger endpoint.
        // the restful api can set up on demand request processing and worker job in the same time.
        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var checkoutOrderCommand = _mapper.Map<CheckOutOrderCommand>(context.Message);

            var newOrderId = await _mediator.Send(checkoutOrderCommand);

            _logger.LogInformation($"Successfully consume and create order with {newOrderId}");


        }
    }
}
