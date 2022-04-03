using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Basket.API.Entities;
using Newtonsoft.Json;


namespace Basket.API.Repositories
{
    public class BasketRepository: IBasketRepository
    {
        // DI which is coming from service registration: AddStackExchangeRedisCache
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var result = await _redisCache.GetStringAsync(userName);
            return result == null ? null : JsonConvert.DeserializeObject<ShoppingCart>(result);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart shoppingCart)
        {       
            var data = JsonConvert.SerializeObject(shoppingCart);

            await _redisCache.SetStringAsync(shoppingCart.UserName, data);

            // reroute logic is put in the business layer, not in the api layer in catalog api
            return await GetBasket(shoppingCart.UserName);
            
        }

        public async Task DeleteBasket(string userName)
        {
            await _redisCache.RemoveAsync(userName);
        }
    }

}
