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
    public class AccountOrderInfo
    {
        public string Id { get; set; }
        public string AccountId { get; set; }

    }
    public class Orders
    {
        public string accountID { get; set; }
        public Orderinfo orderinfo { get; set; }
    }
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Required]
        public string AccountId { get; set; }
        public Orderinfo orderinfo { get; set; }
       // public string Status { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class AccountOrder
    {
        public string AccountId { get; set; }
        public List<Pro> carts { get; set; }
        public AccountOrder() {
            carts = new List<Pro>();
    }
    }
    public class Orderinfo
    {
        public Pro[] carts { get; set; }
        public string Note { get; set; }
        public string ReceiverAdderss { get; set; }
        public DateTime OrderDate { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string CashType { get; set; }
        public string status { get; set; }
    }
}
