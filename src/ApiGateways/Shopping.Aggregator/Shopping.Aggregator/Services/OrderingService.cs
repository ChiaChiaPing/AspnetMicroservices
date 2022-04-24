using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Extensions;

namespace Shopping.Aggregator.Services
{
    public class OrderingService: IOrderingService
    {
        private readonly HttpClient _client;

        public OrderingService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }
    

        public async Task<IEnumerable<OrderResponseModel>> GetOrdersWithUserName(string userName)
        {
            var response = await _client.GetAsync($"/api/v1/Order/{userName}");
            return await response.ReadAsJsonAsync<List<OrderResponseModel>>();
        }
    }
}
