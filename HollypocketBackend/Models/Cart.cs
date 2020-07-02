using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HollypocketBackend.Models
{
    public class Carts
    {
        public P[] product { get; set; }
    }
    public class Cart
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public P[] Products { get; set; }
    }
    public class P
    {
        public string ProductId { get; set; }
        public int Amount { get; set; }
    }
    public class Pro
    {
        public string ProductId { get; set; }
        public int Amount { get; set; }
        public string name { get; set; }
        public string provider { get; set; }
        public decimal price { get; set; }
    }
}
