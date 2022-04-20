using System;
using Microsoft.AspNetCore.Mvc;
using Basket.API.Repositories;
using System.Net;
using Basket.API.Entities;
using System.Threading.Tasks;
using Basket.API.GrpcServices;
using AutoMapper;
using EventBus.Messages;
using MassTransit;

namespace Basket.API.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController: ControllerBase 
    {
        private readonly IBasketRepository _repository;
        private readonly DiscountGrpcServices _discountGrpcServices;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository repository, DiscountGrpcServices discountGrpcServices,
                                IMapper mapper, IPublishEndpoint publishEndpont)
        {

            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _discountGrpcServices = discountGrpcServices ?? throw new ArgumentNullException(nameof(discountGrpcServices));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _publishEndpoint = publishEndpont ?? throw new ArgumentNullException(nameof(publishEndpont));
        }

        [HttpGet("{userName}",Name ="GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var result = await _repository.GetBasket(userName);
            return Ok(result ?? new ShoppingCart());
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart shoppingCart)
        {

            // comunicate and calculate and put
            foreach(var item in shoppingCart.Items)
            {
                var coupon = await _discountGrpcServices.GetDiscount(item.ProuctName);
                item.Price -= coupon.Amount;
            }

            var result = await _repository.UpdateBasket(shoppingCart);
            return Ok(result);
        }

        // if params is not mapped to route like [userName], the userName will be query params not the path parameters
            // case sensitive 
        [HttpDelete("{userName}",Name ="DeleteBasket")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _repository.DeleteBasket(userName);
            return Ok();
        }


        [HttpPost("[action]")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout(BasketCheckout basketCheckout)
        {
            // get the basket
            var basket = await _repository.GetBasket(basketCheckout.UserName);
            if (basket == null)
                return BadRequest();

          
            // convert to the BasketCheckoutEvent through auto mapper which act as Adapter
            var basketCheckOutEvent = _mapper.Map<BasketCheckoutEvent>(basketCheckout);

            // set the total priice to BasketCheckout
            basketCheckOutEvent.TotalPrice = basket.TotalPrice;

            // send the BasketCheckoutEvent to the Message Queue
            await _publishEndpoint.Publish(basketCheckOutEvent);


            // remove the basket in the redis
            await _repository.DeleteBasket(basket.UserName);

            return Accepted();

        }

    }
}
