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
    public class Account
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Required]
        public string Email { get; set; }
        public List<Address> Addresses { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        [Required]
        public string Password { get; set; }
        public string Token { get; set; }
        public AccountType AccountType { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class Address
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string PhoneNumber { get; set; }
    }
}

public enum AccountType
{
    Admin,
    Normal
}
