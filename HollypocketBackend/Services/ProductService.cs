using HollypocketBackend.Models;
using HollypocketBackend.Models.Product;
using Microsoft.AspNetCore.JsonPatch;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HollypocketBackend.Utils;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AutoMapper;
using MongoDB.Driver.GridFS;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders.Physical;

namespace HollypocketBackend.Services
{
    public interface IProductService
    {
        Task<PagedList<Product>> GetWithPage(int pageSize, int pageNumber);
        Task<PagedList<Product>> FilterWithHigherPrice(decimal price, int pageSize, int pageNumber);
        Task<PagedList<Product>> FilterWithLowerPrice(decimal price, int pageSize, int pageNumber);
        Task<PagedList<Product>> FilterWithHigherRate(int rate, int pageSize, int pageNumber);
        Task<PagedList<Product>> FilterWithRangeOfPrice(decimal fromPrice, decimal toPrice, int pageSize, int pageNumber);
        Task<PagedList<Product>> FilterWithRangeOfDiscount(decimal fromDiscount, int pageSize, int pageNumber);
        Task<PagedList<Product>> FilterWithTag(string tagName, int pageSize, int pageNumber);
        Task<PagedList<Product>> FilterWithProvider(string providerName, int pageSize, int pageNumber);
        Task<Product> GetById(string id);
        Task<Product> Create(ProductModel product);
        Task<Product> Delete(string id);
        Task<bool> IsExisting(string id);
        Task PatchUpdate(string id, JsonPatchDocument<UpdateProductModel> patchDoc);
        Task<UpdateProductModel> Update(string id, UpdateProductModel p);
    }
    public class ProductService : IProductService
    {
        private readonly IMongoCollection<Product> _products;
        private IMapper _mapper;
        private GridFSBucket bucket;
        private readonly IMongoDatabase database;
        public ProductService(AppSettings settings, IMapper mapper)
        {
            var client = new MongoClient(settings.ConnectionString);
            database = client.GetDatabase(settings.DatabaseName);

            _products = database.GetCollection<Product>(settings.ProductCollectionName);
            _mapper = mapper;
            this.bucket = new GridFSBucket(database);
        }

        public async Task<PagedList<Product>> GetWithPage(int pageSize, int pageNumber)
        {
            return await PagedList<Product>.ToPagedList(_products.Find(b => true).ToList(), pageSize, pageNumber);
        }

        public async Task<PagedList<Product>> FilterWithProvider(string providerName, int pageSize, int pageNumber)
        {
            return await PagedList<Product>.ToPagedList(_products.Find(p => (p.Provider == providerName)).ToList(), pageSize, pageNumber);
        }
        public async Task<PagedList<Product>> FilterWithTag(string tagName, int pageSize, int pageNumber)
        {
            return await PagedList<Product>.ToPagedList(_products.Find(p => (p.Info.TagName.Contains(tagName))).ToList(), pageSize, pageNumber);
        }
        public async Task<PagedList<Product>> FilterWithHigherRate(int rate, int pageSize, int pageNumber)
        {
            return await PagedList<Product>.ToPagedList(_products.Find(p => (p.Rate >= rate)).SortBy(p => p.Rate).ToList(), pageSize, pageNumber);
        }
        public async Task<PagedList<Product>> FilterWithHigherPrice(decimal price, int pageSize, int pageNumber)
        {
            return await PagedList<Product>.ToPagedList(_products.Find(p => (p.Price >= price)).ToList(), pageSize, pageNumber);
        }
        public async Task<PagedList<Product>> FilterWithLowerPrice(decimal price, int pageSize, int pageNumber)
        {
            return await PagedList<Product>.ToPagedList(_products.Find(p => (p.Price <= price)).ToList(), pageSize, pageNumber);
        }
        public async Task<PagedList<Product>> FilterWithRangeOfPrice(decimal fromPrice, decimal toPrice, int pageSize, int pageNumber)
        {
            return await PagedList<Product>.ToPagedList(_products.Find(p => (p.Price >= fromPrice) && (p.Price <= toPrice)).ToList(), pageSize, pageNumber);
        }
        public async Task<PagedList<Product>> FilterWithRangeOfDiscount(decimal fromDiscount, int pageSize, int pageNumber)
        {
            return await PagedList<Product>.ToPagedList(_products.Find(p => (p.Discount >= fromDiscount)).SortBy(p => p.Discount).ToList(), pageSize, pageNumber);
        }
        public Product[] GetByIds(string[] ids)
        {

            var products = _products.Find(b => true).ToList().Where(p => ids.ToList().Any(p2 => p2 == p.Id)).ToArray();
            return products;
        }

        public async Task<bool> IsExisting(string id)
        {
            var product = await GetById(id);
            if (product != null)
                return true;
            return false;
        }
        public async Task PatchUpdate(string id, JsonPatchDocument<UpdateProductModel> patchDoc)
        {
            var product = await GetById(id);

            var model = _mapper.Map<UpdateProductModel>(product);
            patchDoc.ApplyTo(model);

            await Update(id, model);
        }
        public Task<Product> GetById(string id)
        {
            return _products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Product> Delete(string id)
        {
            var product = await GetById(id);
            await _products.DeleteOneAsync(p => p.Id == id);
            return product;
        }

        public async Task<Product> Create(ProductModel p)
        {
            var product = new Product
            {
                Discount = p.Discount,
                Info = p.Info,
                Price = p.Price,
                ProductName = p.ProductName,
                Provider = p.Provider,
                Questions = p.Questions,
                Rate = p.Rate,
                Pictures = new List<string>(),
                ThumbnailId = ""
            };
            var index = 1;
            //foreach (var file in p.files)
            //{
            //    string idImage = UploadedFile(index, file);
            //    product.Pictures.Add(idImage);
            //    index++;
            //}


            var ThumbnailId = UploadedFile(p.ThumbnailImg.FileName, p.ThumbnailImg);
            product.ThumbnailId = ThumbnailId;

            await _products.InsertOneAsync(product);
            return product;
        }

        public async Task<Product> CreateMock(Product p)
        {

            await _products.InsertOneAsync(p);
            return p;
        }

        public string UploadedFile(int index, IFormFile file)
        {
            var path = file.OpenReadStream();
            var id = bucket.UploadFromStream(index.ToString(), path);

            return id.ToString();
        }

        public string UploadedFile(string fileName, IFormFile file)
        {
            var path = file.OpenReadStream();
            var id = bucket.UploadFromStream(fileName, path);

            return id.ToString();
        }

        public async Task<UpdateProductModel> Update(string id, UpdateProductModel p)
        {
            var product = await GetById(id);
            _mapper.Map(p, product);
            await _products.ReplaceOneAsync(p => p.Id == id, product);
            return p;
        }
        public Product GetId(string Id) => _products.Find(b => b.Id == Id).FirstOrDefault();
    }
}
