using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspnetRunBasics.Models;

namespace AspnetRunBasics.Services
{
    public interface ICatalogService
    {
        Task<IEnumerable<CatalogModel>> GetProducts();
        Task<IEnumerable<CatalogModel>> GetProductsByCategory(string category);
        Task<CatalogModel> GetProductById(string id);
        Task<CatalogModel> CreateCatalog(CatalogModel model);

    }
}
