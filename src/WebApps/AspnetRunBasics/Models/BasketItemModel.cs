using System;
namespace AspnetRunBasics.Models
{
    public class BasketItemModel
    {
        public int ProductId { get; set; }
        public string ProuctName { get; set; }
        public string Color { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    
    }
}
