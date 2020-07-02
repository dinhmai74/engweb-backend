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
    public class FavoriteService
    {
        private readonly IMongoCollection<Favorite> _favorites;
        private readonly AppSettings _appSettings;
        public FavoriteService(AppSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var datatbase = client.GetDatabase(settings.DatabaseName);
            _appSettings = settings;
            _favorites = datatbase.GetCollection<Favorite>(settings.FavoriteCollectionName);
        }
        public List<Favorite> Get() => _favorites.Find(b => true).ToList();
        public Favorite GetByUser() => _favorites.Find(b => true).First();

        public Favorite Get(string id) => _favorites.Find(b => b.Id == id).FirstOrDefault();
        public Favorite GetById(string Id) => _favorites.Find(b => b.UserId == Id).FirstOrDefault();
        public Favorite Insert(Favorite p)
        {
            _favorites.InsertOne(p);
            return p;
        }
        public void Update(string id, Favorite favorite) => _favorites.ReplaceOne(p => p.Id == id, favorite);
        public void Delete(Favorite favorite) => _favorites.DeleteOne(b => b.Id == favorite.Id);
        public void Delete(string id) => _favorites.DeleteOne(b => b.Id == id);
    }
}
