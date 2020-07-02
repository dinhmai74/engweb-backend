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
    public class CartService
    {
        private readonly IMongoCollection<Cart> _carts;
        private readonly AppSettings _appSettings;
        public CartService(AppSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var datatbase = client.GetDatabase(settings.DatabaseName);

            _appSettings = settings;
            _carts = datatbase.GetCollection<Cart>(settings.CartsCollectionName);
        }
        public List<Cart> Get() => _carts.Find(b => true).ToList();
        public Cart Get(string Id) => _carts.Find(b => b.Id == Id).FirstOrDefault();
        public Cart GetUserId(string id) => _carts.Find(b => b.UserId == id).FirstOrDefault();
        public Cart Insert(Cart cart)
        {
            var existedCart = _carts.Find(b => b.UserId == cart.UserId).FirstOrDefault();
            if (existedCart == null) _carts.InsertOne(cart);
            else
            {
                foreach (var value in cart.Products)
                {
                    // existedCart.Products.Append(p);
                    var product = existedCart.Products.ToList().Find(p => p.ProductId == value.ProductId);
                    if (product != null)
                    {
                        product.Amount++;
                    }
                    else
                    {
                        var listProducts = existedCart.Products.ToList();
                        listProducts.Add(value);
                        existedCart.Products = listProducts.ToArray();
                    }
                }

                Update(existedCart.Id, existedCart);

            }
            return cart;
        }
        public void Update(string id, Cart cart) => _carts.ReplaceOne(p => p.Id == id, cart);
        public void Delete(Cart cart) => _carts.DeleteOne(b => b.Id == cart.Id);
        public void Delete(string id) => _carts.DeleteOne(b => b.Id == id);
    }
}
