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
    public class AccountService
    {
        private readonly IMongoCollection<Account> _accounts;
        private readonly AppSettings _appSettings;

        public AccountService(AppSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _appSettings = settings;
            _accounts = database.GetCollection<Account>(settings.AccountsCollectionName);
        }

        public Account Authenticate(string email, string password)
        {
            var user = _accounts.Find(b => b.Email == email && b.Password == password).FirstOrDefault();
            if (user == null) return null;
            // authentication successful so generate jwt token
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.AccountType.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return user.WithoutPassword();
        }

        public Boolean CheckValidPassword(string email, string password)
        {
            var user = _accounts.Find(b => b.Email == email && b.Password == password).FirstOrDefault();
            if (user == null) return false;
            return true;
        }

        public List<Account> Get() => _accounts.Find(b => true).ToList();

        public Account Get(string id) => _accounts.Find(b => b.Id == id).FirstOrDefault();
        public Account GetByEmail(string email) => _accounts.Find(b => b.Email == email).FirstOrDefault();

        public Account Insert(Account p)
        {

            _accounts.InsertOne(p);
            return p;
        }

        public void Update(string id, Account account) => _accounts.ReplaceOne(p => p.Id == id, account);

        public void Delete(Account account)
        {
            account.IsDeleted = true;
            _accounts.ReplaceOne(p => p.Id == account.Id, account);
        }
        public void Delete(string id)
        {
            var account = _accounts.Find(b => b.Id == id).FirstOrDefault();
            account.IsDeleted = true;
            _accounts.ReplaceOne(p => p.Id == account.Id, account);
        }
    }
}
