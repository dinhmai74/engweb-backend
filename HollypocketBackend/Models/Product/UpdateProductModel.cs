using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HollypocketBackend.Models.Product
{
    public class UpdateProductModel
    {

        [BsonElement("Name")]
        public string ProductName { get; set; }
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }

        public string Provider { get; set; }

        public int Rate { get; set; }
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Discount { get; set; }

        public string[] Questions { get; set; }
        public List<string> Pictures { get; set; }
        public Info Info { get; set; }
    }
}
