using System;
using System.Threading.Tasks;
using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Discount.API.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly ILogger<DiscountController> _logger;
        private readonly IDiscountRepository _discountRepository;

        
        public DiscountController(ILogger<DiscountController> logger, IDiscountRepository discountRepository)
        {

            _logger = logger;
            _discountRepository = discountRepository;
        }

        [HttpGet("{productName}", Name = "GetCoupon")]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> GetCoupon(string productName)
        {
            var coupon = await _discountRepository.GetCoupon(productName);
            return Ok(coupon);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Coupon>> CreateCoupon([FromBody] Coupon coupon)
        {
            var result = await _discountRepository.CreateCoupon(coupon);
            return CreatedAtAction("GetCoupon", new { productName = coupon.ProductName }, coupon);
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> UpdateCoupon([FromBody] Coupon coupon)
        {
            var result = await _discountRepository.UpdateCoupon(coupon);
            return Ok(result);
            
        }

        [HttpDelete("{productName}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteCoupon(string productName)
        {
            var result = await _discountRepository.DeleteCoupon(productName);
            return Ok(result);
        }
    }
}
