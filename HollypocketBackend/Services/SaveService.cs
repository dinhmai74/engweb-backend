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
    public class SaveService
    {
        private readonly IMongoCollection<Save> _saves;
        private readonly AppSettings _appSettings;
        public SaveService(AppSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var datatbase = client.GetDatabase(settings.DatabaseName);

            _appSettings = settings;
            _saves = datatbase.GetCollection<Save>(settings.SavesCollectionName);
        }
        public List<Save> Get() => _saves.Find(b => true).ToList();
        public Save Get(string Id) => _saves.Find(b => b.Id == Id).FirstOrDefault();
        public Save GetUserId(string id) => _saves.Find(b => b.UserId == id).FirstOrDefault();
        public Save Insert(Save p)
        {
            _saves.InsertOne(p);
            return p;
        }
        public void Update(string id, Save cart) => _saves.ReplaceOne(p => p.Id == id, cart);
        public void Delete(Save cart) => _saves.DeleteOne(b => b.Id == cart.Id);
        public void Delete(string id) => _saves.DeleteOne(b => b.Id == id);
    }
}
