using System;
using Catalog.API.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;


namespace Catalog.API.Data
{
    public class CatalogContext: ICatalogContext
    {

        public IMongoCollection<Product> Products { get; }


        // DI confiiguration, can ingest the settings from appsettings

        public CatalogContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

            Products = database.GetCollection<Product>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));

            // initialize the data if the Product Collection is not found.

            CatalogContextData.SeedData(Products);

        }

        
    }
}
