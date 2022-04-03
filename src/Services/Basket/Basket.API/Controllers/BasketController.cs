using System;
using Microsoft.AspNetCore.Mvc;
using Basket.API.Repositories;
using System.Net;
using Basket.API.Entities;
using System.Threading.Tasks;


namespace Basket.API.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController: ControllerBase 
    {
        private readonly IBasketRepository _repository;

        public BasketController(IBasketRepository repository)
        {

            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
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
            var result = await _repository.UpdateBasket(shoppingCart);
            return Ok(result );
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
