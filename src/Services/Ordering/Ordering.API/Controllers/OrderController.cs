using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands.CheckOutOrder;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries;

namespace Ordering.API.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {

            _mediator = mediator;

        }

        [HttpGet("{userName}",Name = "GetOrdersList")]
        [ProducesResponseType(typeof(List<OrdersVm>),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<OrdersVm>>> GetOrdersList(string userName)
        {
            var query = new GetOrdersListQuery(userName);
            var ordersList = await _mediator.Send(query);
            return Ok(ordersList);

        }

        [HttpPost(Name = "CheckOutOrder")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> CheckOutOurder(CheckOutOrderCommand command)
        {        
            var orderId = await _mediator.Send(command);
            return Ok(orderId);

        }

        [HttpPut(Name = "UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateOrder(UpdateOrderCommand command)
        {
            await _mediator.Send(command);

            // 不要看到 No content 就以為是錯的 其實也可以是正確的 因為no return type, 所以對action result 而言不用回傳任何內容
            // 反而是可以用no content 代表command 成功, 因為我們還有另外一個notfound exception, 故此區分
            return NoContent();
            
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var command = new DeleteOrderCommand() { Id = id };
            await _mediator.Send(command);
            return NoContent();
           

        }
    }
}
