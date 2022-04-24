using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services
{
    public interface ICatalogService
    {
       
        Task<IEnumerable<CatalogModel>> GetProducts();
        Task<IEnumerable<CatalogModel>> GetProductsByCategory(string category);
        Task<CatalogModel> GetProductById(string id);

    }
}
