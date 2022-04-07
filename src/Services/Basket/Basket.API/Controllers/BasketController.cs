using System;
using Microsoft.AspNetCore.Mvc;
using Basket.API.Repositories;
using System.Net;
using Basket.API.Entities;
using System.Threading.Tasks;
using Basket.API.GrpcServices;
using Discount.Grpc.Protos;


namespace Basket.API.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController: ControllerBase 
    {
        private readonly IBasketRepository _repository;
        private readonly DiscountGrpcServices _discountGrpcServices;

        public BasketController(IBasketRepository repository, DiscountGrpcServices discountGrpcServices)
        {

            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _discountGrpcServices = discountGrpcServices ?? throw new ArgumentNullException(nameof(discountGrpcServices));
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

    }
}
