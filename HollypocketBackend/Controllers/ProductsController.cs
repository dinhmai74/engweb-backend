using HollypocketBackend.Models;
using HollypocketBackend.Models.Product;
using HollypocketBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Faker;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.IO;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace HollypocketBackend.Controllers
{
    [Route("api/v1/[controller]")]
    [CustomExceptionFilter]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult> GetProductWithPage(int pageNumber = 1, int pageSize = 10)
        {
            var apiRep = new APIResponse();
            var products = await _productService.GetWithPage(pageSize, pageNumber);
            apiRep.Error = false;
            apiRep.Data = products;
            return Ok(apiRep);
        }

        [HttpGet("get/{productId}")]
        public async Task<ActionResult> GetById(string productId)
        {

            var apiRep = new APIResponse();
            var product = await _productService.GetById(productId);

            apiRep.Error = false;
            apiRep.Data = product;

            return Ok(apiRep);
        }

        [HttpGet("get-products-with-id")]
        public ActionResult GetByIds([FromQuery] string[] ids)
        {

            var apiRep = new APIResponse();
            var products = _productService.GetByIds(ids);
            apiRep.Error = false;
            apiRep.Data = products;

            return Ok(apiRep);
        }
        [HttpGet("Get-With-Rate/{Rate}")]
        public async Task<ActionResult> GetWithRate(int rate, int pageNumber = 1, int pageSize = 10)
        {

            var apiRep = new APIResponse();
            var products = await _productService.FilterWithHigherRate(rate, pageSize, pageNumber);
            apiRep.Error = false;
            apiRep.Data = products;
            return Ok(apiRep);
        }

        [HttpGet("Get-With-Provider/{providerName}")]
        public async Task<ActionResult> GetWithProvider(string providerName, int pageNumber = 1, int pageSize = 10)
        {

            var apiRep = new APIResponse();
            var products = await _productService.FilterWithProvider(providerName, pageSize, pageNumber);
            apiRep.Error = false;
            apiRep.Data = products;
            return Ok(apiRep);
        }

        [HttpGet("Get-With-Higher-Price/{price}")]
        public async Task<ActionResult> GetWithHigherPrice(decimal price, int pageNumber = 1, int pageSize = 10)
        {

            var apiRep = new APIResponse();
            var products = await _productService.FilterWithHigherPrice(price, pageSize, pageNumber);
            apiRep.Error = false;
            apiRep.Data = products;
            return Ok(apiRep);
        }

        [HttpGet("Get-With-Range-Price")]
        public async Task<ActionResult> GetWithRangeOfPrice(decimal fromPrice, decimal toPrice, int pageNumber = 1, int pageSize = 10)
        {

            var apiRep = new APIResponse();
            var products = await _productService.FilterWithRangeOfPrice(fromPrice, toPrice, pageSize, pageNumber);
            apiRep.Error = false;
            apiRep.Data = products;
            return Ok(apiRep);
        }

        [HttpGet("Get-With-Higher-Discount/{discount}")]
        public async Task<ActionResult> GetWithHigherDiscount(decimal discount, int pageNumber = 1, int pageSize = 10)
        {

            var apiRep = new APIResponse();
            var products = await _productService.FilterWithRangeOfDiscount(discount, pageSize, pageNumber);
            apiRep.Error = false;
            apiRep.Data = products;
            return Ok(apiRep);
        }

        [HttpGet("Get-With-Lower-Price/{price}")]
        public async Task<ActionResult> GetWithLowerPrice(decimal price, int pageNumber = 1, int pageSize = 10)
        {

            var apiRep = new APIResponse();
            var products = await _productService.FilterWithLowerPrice(price, pageSize, pageNumber);
            apiRep.Error = false;
            apiRep.Data = products;
            return Ok(apiRep);
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromQuery] ProductModel product)
        {

            var apiRep = new APIResponse();
            apiRep.Error = false;
            apiRep.Data = await _productService.Create(product);

            return Ok(apiRep);
        }

        [HttpPost("update/{productId}")]
        public async Task<IActionResult> Update(string productId, UpdateProductModel product)
        {
            var apiRep = new APIResponse();
            if (!(await _productService.IsExisting(productId)))
            {
                apiRep.Data = null;
                apiRep.Error = false;

                return Ok(apiRep);
            }
            var p = await _productService.Update(productId, product);
            apiRep.Data = p;
            apiRep.Error = false;
            return Ok(apiRep);
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(string productId)
        {
            var apiRep = new APIResponse();
            if (!(await _productService.IsExisting(productId)))
            {
                apiRep.Data = null;
                apiRep.Error = false;

                return Ok(apiRep);
            }

            var p = await _productService.Delete(productId);

            apiRep.Data = p;
            apiRep.Error = false;

            return Ok(apiRep);
        }

        [HttpGet("generate-mock-data/{times}")]
        public async Task<IActionResult> GenerateMockData(Int64 times)
        {
            var apiRep = new APIResponse();
            string[] providerNames = { "Wolsen", "HuonGoo", " I’m Game", "Rongyuxuan", "Sony", "Nvidia Shield", "LeapFrog", "Nintendo" };
            string[] generations = { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X" };
            string[] tags = { "Console", "Hot", "Sale off", };

            using var httpClient = new HttpClient();

            // // Get the file extension
            // var uriWithoutQuery = uri.GetLeftPart(UriPartial.Path);
            // var fileExtension = Path.GetExtension(uriWithoutQuery);

            // // Create file path and ensure directory exists
            // var path = Path.Combine(directoryPath, $"{fileName}{fileExtension}");
            // Directory.CreateDirectory(directoryPath);

            for (int i = 0; i < times; i++)
            {

                Random r = new Random();


                var p = new Product();
                p.Price = r.Next(50, 500);
                var providerName = providerNames[r.Next(0, providerNames.Length)];
                p.Provider = providerName;

                var name = providerName + " " + Faker.Name.Last() + " " + generations[r.Next(0, generations.Length)];
                p.ProductName = name;
                p.Rate = r.Next(1, 5);
                var listQuestions = new List<string>();
                p.Questions = listQuestions.ToArray();
                string afterPoint = r.Next(0, 6).ToString();//1st decimal point
                //string secondDP = r.Next(0, 9).ToString();//2nd decimal point
                string combined = 0 + "." + afterPoint;
                Decimal decimalNumber = Decimal.Parse(combined);
                p.Discount = decimalNumber;

                // p.Discount= Faker.RandomNumber.;

                string[] tag = { providerName, tags[r.Next(0, tags.Length)] };
                // p.Info.TagName = tag;

                string[] Descriptions = Faker.Lorem.Paragraphs(20).ToArray();
                // p.Info.Descriptions = Descriptions;

                p.Info = new Info
                {
                    Descriptions = Descriptions,
                    TagName = tag
                };

                // images
                var uri = "https://picsum.photos/200/" + r.Next(200, 400);
                // Download the image and write to the file
                var client = new MongoClient("mongodb://localhost:27017");
                var imageBytes = await httpClient.GetByteArrayAsync(uri);
                var database = client.GetDatabase("HollyStoreDb");
                var bucket = new GridFSBucket(database);
                var idImage = Faker.Identification.SocialSecurityNumber();
                var id = bucket.UploadFromBytes(idImage.ToString(), imageBytes).ToString();
                var Pictures = new List<string>();

                for (int j = 0; j < 5; j++)
                {
                    // images
                    var ptUri = "https://picsum.photos/200/" + r.Next(200, 400);
                    // Download the image and write to the file
                    var img = await httpClient.GetByteArrayAsync(uri);
                    var tempId = bucket.UploadFromBytes(ptUri, img).ToString();
                    Pictures.Add(tempId);
                }

                p.ThumbnailId = id;
                p.Pictures = Pictures;
                await _productService.CreateMock(p);
            }

            apiRep.Error = false;
            apiRep.Data = true;
            return Ok(apiRep);
        }
    }
}
