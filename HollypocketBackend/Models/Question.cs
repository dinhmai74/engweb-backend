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
    public class Question
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]

        //userid 
        public string Id { get; set; }
        public string userId { get; set; }
        public string productId { get; set; }
        public string Answer { get; set; }
        public string title { get; set; }
        public string decriptions { get; set; }

    }
}
