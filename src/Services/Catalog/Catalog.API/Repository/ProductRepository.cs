using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.API.Entities;
using Catalog.API.Data;
using System.Linq;
using MongoDB.Driver;

namespace Catalog.API.Repository
{
    public class ProductRepository: IProductRepository
    {

        // Initialize the connection string through DI. 
        private readonly ICatalogContext _context;
        
        // constructor DI
        public ProductRepository(ICatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Product> GetProductById(string id)
        {
            return await _context.Products
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products
                           .Find(p => true)
                           .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(string category)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, category);
            return await _context.Products
                                .Find(filter)
                                .ToListAsync();

        }

        public async Task<IEnumerable<Product>> GetProductsByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);
            return await _context.Products
                                .Find(filter)
                                .ToListAsync();
        }

        public Task CreateProduct(Product product)
        {
            return _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProduct(string id)
        {
            var deleteResult = await _context.Products.DeleteOneAsync(p => p.Id == id);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;

        }

        public async Task<bool> UpdateProduct(Product product)
        {

            var updateResult = await _context.Products.ReplaceOneAsync(filter: p => p.Id == product.Id, replacement: product);

            return updateResult.IsAcknowledged &&
                    updateResult.ModifiedCount > 0;
            
        }
    }
}
