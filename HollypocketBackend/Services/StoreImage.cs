using HollypocketBackend.Models;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HollypocketBackend.Services
{
    public class StoreImage
    {
        private readonly IMongoDatabase database;
        private GridFSBucket bucket;

        public StoreImage(AppSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            this.database = client.GetDatabase(settings.DatabaseName);

            this.bucket = new GridFSBucket(database);
        }

        public string UploadedFile(IFormFile file)
        {
            var path = file.OpenReadStream();
            var id = bucket.UploadFromStream(file.FileName, path);

            return id.ToString();
        }

        public byte[] DownloadFile(string id)
        {
            return bucket.DownloadAsBytes(new ObjectId(id));
        }

    }
}
