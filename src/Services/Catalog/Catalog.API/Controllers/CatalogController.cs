using System;
using Microsoft.Extensions.Logging;
using Catalog.API.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Catalog.API.Entities;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{

    // Basic Rooute
    // [controller] => extract Class Name and dismiss the Controller keyword
    [ApiController]
    [Route("api/v1/[controller]")] 
    public class CatalogController : ControllerBase
    {
        // logger record the steps for the logic within CatalogController
        private readonly ILogger<CatalogController> _logger;
        private readonly IProductRepository _repository;


        // DI will be automatically after service registration
        public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }


        // HTTP Method
        // Define the HTTP Response type of value and code called by this action
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _repository.GetProducts();

            return Ok(products);
        }

        // HTTPMethod Get define the params. the name is just meta data for the rouute
        // HTTPGET("{id}") is appended resource endpoint ../{id}
        [HttpGet("{id:length(24)}", Name = "GetProductById")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await _repository.GetProductById(id);

            if (product == null)
            {
                _logger.LogError($"cannot find the data based on the {id}");
                return NotFound();
            }

            return Ok(product);
        }

        // can define tthe route. [] will extract the name from the MethodName, {} is like the parameter that used in the method
        [Route("[action]/{category}", Name = "GetProductByCategory")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
        {
            var products = await _repository.GetProductsByCategory(category);

            if (products == null)
            {
                _logger.LogError($"cannot find the data based on the {category}");
                return NotFound();
            }

            return Ok(products);
        }

        [Route("[action]/{name}", Name = "GetProductByName")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByName(string name)
        {
            var products = await _repository.GetProductsByName(name);

            if (products == null)
            {
                _logger.LogError($"cannot find the data based on the {name}");
                return NotFound();
            }

            return Ok(products);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await _repository.CreateProduct(product);

            // route and all another api method based on the RouteName. call GetProductById.
            return CreatedAtRoute("GetProductById", new { id = product.Id }, product);
        }

        // if no addition action or no need to return the specific type of value want to do then can use IActionResult
        [HttpPut]
        [ProducesResponseType(typeof(bool), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            var updateResult = await _repository.UpdateProduct(product);

            return Ok(updateResult);

        }

        [HttpDelete("{id:length(24)}",Name = "DeleteProduct")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var deleteResult = await _repository.DeleteProduct(id);
            return Ok(deleteResult);

        }

    }

   
}
