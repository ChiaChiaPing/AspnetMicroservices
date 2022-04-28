using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AspnetRunBasics.Models;
using AspnetRunBasics.Extensions;

namespace AspnetRunBasics.Services
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
            var response = await _client.GetAsync($"/Order/{userName}");
            return await response.ReadAsJsonAsync<List<OrderResponseModel>>();
            
        }
    }
}
