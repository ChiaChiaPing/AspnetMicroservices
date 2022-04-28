using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AspnetRunBasics.Models;
using AspnetRunBasics.Extensions;

namespace AspnetRunBasics.Services
{
    public class BasketService: IBasketService
    {
        private readonly HttpClient _client;

        public BasketService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));

        }

        public async Task CheckoutBasket(BasketCheckoutModel model)
        {
            var response = await _client.PostAsJsonAsync("/Basket/Checkout", model);
            if (response.IsSuccessStatusCode)
            {
                return;
            }
            else
            {
                throw new Exception("Something when invoking the api");
            }

        }

        public async Task<BasketModel> GetBasket(string userName)
        {
            var response = await _client.GetAsync($"/Basket/{userName}");
            return await response.ReadAsJsonAsync<BasketModel>();

            
        }

        public async Task<BasketModel> UpdateBasket(BasketModel model)
        {
            var response = await _client.PostAsJsonAsync("/Basket", model);
            if (response.IsSuccessStatusCode)
            {
                return await response.ReadAsJsonAsync<BasketModel>();
            }
            else
            {
                throw new Exception("Something when invoking the api");
            }
        }
    }
}
