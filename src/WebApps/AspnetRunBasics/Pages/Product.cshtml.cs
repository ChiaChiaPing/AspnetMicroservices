using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics
{
    public class ProductModel : PageModel
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;

        public ProductModel(ICatalogService catalogService, IBasketService basketService)
        {
            _catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
            _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
        }

        public IEnumerable<string> CategoryList { get; set; } = new List<string>();
        public IEnumerable<CatalogModel> ProductList { get; set; } = new List<CatalogModel>();


        [BindProperty(SupportsGet = true)]
        public string SelectedCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(string categoryName)
        {
            var products = await _catalogService.GetProducts();
            CategoryList = products.Select(p => p.Category).Distinct();

            if (!string.IsNullOrWhiteSpace(categoryName))
            {
                ProductList = products.Where(p => p.Category == categoryName);
                SelectedCategory = categoryName;
                
            }
            else
            {
                // if no category Name is taken then take all products
                ProductList = products;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            //if (!User.Identity.IsAuthenticated)
            //    return RedirectToPage("./Account/Login", new { area = "Identity" });

            var product = await _catalogService.GetProductById(productId);
            var userName = "swn";
            var updatedBasket = await _basketService.GetBasket(userName);
            updatedBasket.UserName = userName;

            updatedBasket.Items.Add(new BasketItemModel()
            {
                ProductId = int.Parse(product.Id),
                ProuctName = product.Name,
                Price = product.Price,
                Color = "Black",
                Quantity = 1
            });

            await _basketService.UpdateBasket(updatedBasket);
            return RedirectToPage("Cart");
        }
    }
}