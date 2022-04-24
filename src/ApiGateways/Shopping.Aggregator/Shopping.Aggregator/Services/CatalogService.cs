using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services
{
    public class CatalogService: ICatalogService
    {
        private readonly HttpClient _client;

        public CatalogService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<CatalogModel> GetProductById(string id)
        {
            var response = await _client.GetAsync($"/api/v1/Catalog/{id}");
            return await response.ReadAsJsonAsync<CatalogModel>();
        }

        public async Task<IEnumerable<CatalogModel>> GetProductsByCategory(string category)
        {
            var response = await _client.GetAsync($"/api/v1/Catalog/GetProductByCategory/{category}");
            return await response.ReadAsJsonAsync<List<CatalogModel>>();
        }

        public async Task<IEnumerable<CatalogModel>> GetProducts()
        {
            var response = await _client.GetAsync("/api/v1/Catalog");
            return await response.ReadAsJsonAsync<List<CatalogModel>>();
        }
    }
}
