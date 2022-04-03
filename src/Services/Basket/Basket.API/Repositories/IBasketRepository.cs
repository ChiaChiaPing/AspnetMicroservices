using System;
using Basket.API.Entities;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
    public interface IBasketRepository
    {
        public Task<ShoppingCart> GetBasket(string userName);

        // cuz when add/update, will just overwrite the value with username key
        public Task<ShoppingCart> UpdateBasket(ShoppingCart shoppingCart);

        public Task DeleteBasket(string userName);
    }
}
