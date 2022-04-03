using System;
namespace Basket.API.Entities
{
    public class ShoppingCartItem
    {
        public int ProductId { get; set; }
        public string ProuctName { get; set;}
        public string Color { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }


    }
}