using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;

namespace Shopping.Aggregator.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ShoppingController : ControllerBase
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;
        private readonly IOrderingService _orderingService;

        public ShoppingController(ICatalogService catalogService, IBasketService basketService,
                                IOrderingService orderingService)

        {
            _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
            _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
            _orderingService = orderingService ?? throw new ArgumentNullException(nameof(orderingService));

        }

        [HttpGet("{userName}", Name = "GetShopping")]
        [ProducesResponseType(typeof(ShoppingModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingModel>> GetShopping(string userName)
        {
            
            // if the json respone didn't contain the prop in the converted DTO, then will assign null for those props
            var basket = await _basketService.GetBasket(userName);

            foreach(var item in basket.Items)
            {
                var product = await _catalogService.GetProductById(item.ProductId.ToString());

                
                item.Description = product.Description;
                item.ImageFile = product.ImageFile;
                item.Summary = product.Summary;
                item.Category = product.Category;

            }

            var orders = await _orderingService.GetOrdersWithUserName(userName);

            var shoppingModel = new ShoppingModel()
            {
                UserName = userName,
                BasketWithProducts = basket,
                Orders = orders

            };
            return Ok(shoppingModel);
        }
    }
}
