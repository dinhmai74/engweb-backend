using HollypocketBackend.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HollypocketBackend.Services
{
    public class RateService
    {
        private readonly IMongoCollection<Rate> _rates;

        private readonly AppSettings _appSettings;
        public RateService(AppSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _appSettings = settings;
            _rates = database.GetCollection<Rate>(settings.RateCollectionName);
        }

        public List<Rate> Get() =>
        _rates.Find(Rate => true).ToList();

        public Rate Get(string id) =>
        _rates.Find<Rate>(b => b.Id == id).FirstOrDefault();


        public Rate Create(Rate rate)
        {
            _rates.InsertOne(rate);
            return rate;
        }

        public void Update(string id, Rate rateIn) =>
        _rates.ReplaceOne(rate => rate.Id == id, rateIn);

        public void Delete(Rate rateIn) => _rates.DeleteOne(b => b.Id == rateIn.Id);
        public void Delete(string id) => _rates.DeleteOne(b => b.Id == id);
        public float Average(string productId)
        {
            float sum=0;
            var rates = _rates.Find(r => r.productId == productId).ToList();
            foreach(var item in rates)
            {
                sum +=(float)(item.rate.ValueRating);
            }
            return (sum / (rates.LongCount()));
        }
    }
}
