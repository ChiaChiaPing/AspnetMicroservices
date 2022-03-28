using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Catalog.API.Entities;

namespace Catalog.API.Repository
{

    //like DAO define the interface of Business Layer
    public interface IProductRepository 
    {

        Task<IEnumerable<Product>> GetProducts();
        Task<IEnumerable<Product>> GetProductsByName(string name);
        Task<IEnumerable<Product>> GetProductsByCategory(string category);
        Task<Product> GetProductById(string id);
        Task CreateProduct(Product product);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(string id);
        
    }
}
