using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;


/*Entity like DTO*/
namespace Catalog.API.Entities
{
    public class Product
    {
        // document id, need id
        // Binary Json
        [BsonId] 
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }

        
        [BsonElement("Name")]
        public string Name { get; set; }

        public string Category { get; set; }

        public string Summary { get; set; }

        public string Description { get; set; }

        public string ImageFile { get; set; }

        public decimal Price { get; set; }
    }
}
