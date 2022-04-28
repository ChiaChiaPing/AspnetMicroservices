using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AspnetRunBasics.Models;
using AspnetRunBasics.Extensions;

namespace AspnetRunBasics.Services
{
    public class CatalogService: ICatalogService
    {
        private readonly HttpClient _client;

        public CatalogService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<CatalogModel> CreateCatalog(CatalogModel model)
        {
            var response = await _client.PostAsJsonAsync("/Catalog", model);
            if (response.IsSuccessStatusCode)
            {
                return await response.ReadAsJsonAsync<CatalogModel>();
            }
            else
            {
                throw new Exception("Something when invoking the api");
            }
        }

        public async Task<CatalogModel> GetProductById(string id)
        {
            var response = await _client.GetAsync($"/Catalog/{id}");
            return await response.ReadAsJsonAsync<CatalogModel>();
        }

        public async Task<IEnumerable<CatalogModel>> GetProducts()
        {
            var response = await _client.GetAsync($"/Catalog");
            return await response.ReadAsJsonAsync<List<CatalogModel>>();
        }

        public async Task<IEnumerable<CatalogModel>> GetProductsByCategory(string category)
        {
            var response = await _client.GetAsync($"/Catalog/GetProductByCategory/{category}");
            return await response.ReadAsJsonAsync<List<CatalogModel>>();
        }
    }
}
