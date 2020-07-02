using System.Net;
using HollypocketBackend.Models;
using HollypocketBackend.Utils;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HollypocketBackend.Services
{
    public class OrderService
    {
        private readonly IMongoCollection<Order> _order;
        private readonly AppSettings _appSettings;

        public OrderService(AppSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var datatbase = client.GetDatabase(settings.DatabaseName);

            _appSettings = settings;
            _order = datatbase.GetCollection<Order>(settings.OrdersCollectionName);
        }
        public List<Order> Get() => _order.Find(b => true).ToList();

        public Order Get(string id) => _order.Find(b => b.Id == id).FirstOrDefault();
        public List<Order> GetById(string Id) => _order.Find(b => b.AccountId == Id).ToList();

        public Order Insert(Order p)
        {

            _order.InsertOne(p);
            return p;
        }

        public void Update(string id, Order order) => _order.ReplaceOne(p => p.Id == id, order);

        public void Delete(Order order)
        {
            order.IsDeleted = true;
            _order.ReplaceOne(p => p.Id == order.Id, order);
        }
        public void Delete(string id) => _order.DeleteOne(b => b.Id == id);
    }
}
