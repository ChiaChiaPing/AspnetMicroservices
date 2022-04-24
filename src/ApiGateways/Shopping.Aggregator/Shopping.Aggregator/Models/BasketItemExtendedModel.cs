using System;
namespace Shopping.Aggregator.Models
{
    public class BasketItemExtendedModel
    {
        public int ProductId { get; set; }
        public string ProuctName { get; set; }
        public string Color { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        // additional info from catalog product
        public string Category { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string ImageFile { get; set; }


    }
}
