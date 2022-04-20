using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Ordering.Application.Features.Orders.Commands.CheckOutOrder
{
    public class CheckOutOrderCommandHandler: IRequestHandler<CheckOutOrderCommand,int>
    {

        private readonly IMapper _mapper;
        private readonly ILogger<CheckOutOrderCommandHandler> _logger;
        private readonly IOrderRepository _orderRepository;
        private readonly IEmailService _emailService;


        public CheckOutOrderCommandHandler(
            IOrderRepository orderRepository,
            IEmailService emailService,
            IMapper mapper,
            ILogger<CheckOutOrderCommandHandler> logger)
        {

            _mapper = mapper;
            _orderRepository = orderRepository;
            _emailService = emailService;
            _logger = logger;

        }


        // save the order and send the mail
        public async Task<int> Handle(CheckOutOrderCommand request, CancellationToken cancellationToken)
        {
            // convert command obj too order obj
            var newOrder = _mapper.Map<Order>(request);

            // save the order to sql database
            var newOrderCreated  = await _orderRepository.AddAsync(newOrder);

            _logger.LogInformation($"The new order is created with {newOrderCreated.Id}");

            // send the mail
            await SendEmail(newOrderCreated);

            // return new created id
            return newOrderCreated.Id;
            
            
        }

        private async Task SendEmail(Order order)
        {

            Email email = new Email () {
                To = "kevinchia@gmail.com",
                Subject = $"The order is created with {order.Id}",
                Body = $"The order is created with {order.Id}"
            };

            try
            {
                await _emailService.SentEmail(email);
                _logger.LogInformation($"Email is sent to {email.To} Successfully");
            }
            catch (Exception e)
            {
                _logger.LogError($"Email is sent to {email.To} Fail due to {e.Message}");
            }

        }
    }
}
