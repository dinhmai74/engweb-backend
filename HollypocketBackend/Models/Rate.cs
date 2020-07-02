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
    public class Rate{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        // Cai [BsonRepresentation(BsonType.ObjectId)] bên dưới là Field Id, 2 cái viết liền nhau
        public string userId {get;set;}
        public string productId { get; set; }
        public RateInfo rate {get; set;}
    }

    public class RateInfo {
        public float ValueRating {get;set;}
        public string Comment{get;set;}
    }
}
