using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HollypocketBackend.Models.Product
{
    public class ProductModel
    {
        public string ProductName { get; set; }
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }

        public string Provider { get; set; }

        // rating over 100
        public int Rate { get; set; }
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Discount { get; set; }
        public string[] Questions { get; set; }
        public List<IFormFile> files { get; set; }
        public IFormFile ThumbnailImg { get; set; }
        //public IFormFile file { get; set; }
        public Info Info { get; set; }
    }
}
