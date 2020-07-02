using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HollypocketBackend.Models;
using HollypocketBackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HollypocketBackend.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly StockService _stockService;

        public WarehouseController(StockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult> GetStockWithPageProductId(string productId,[FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var apiRep = new APIResponse();
            var stocks = await _stockService.GetWithProductId(productId, pageSize, pageNumber);
            apiRep.Error = false;
            apiRep.Data = stocks;
            return Ok(apiRep);
        }

        [HttpPost("create-stock-with-quantity")]
        public async Task<ActionResult> GetStockWithPageProductId(string productId, int quantity)
        {
            var apiRep = new APIResponse();
            var stocks = await _stockService.CreateStockWithQuantity(productId, quantity);
            apiRep.Error = false;
            apiRep.Data = stocks;
            return Ok(apiRep);
        }

        [HttpPost("sell-with-quantity")]
        public async Task<ActionResult> SellWithQuantity(string productId, int quantity)
        {
            var apiRep = new APIResponse();
            var stocks = await _stockService.SoldStock(productId, quantity);
            apiRep.Error = false;
            apiRep.Data = stocks;
            return Ok(apiRep);
        }

        [HttpGet("get-quantity")]
        public async Task<ActionResult> SellWithQuantity(string productId)
        {
            var apiRep = new APIResponse();
            var stocks = await _stockService.CountInStock(productId);
            apiRep.Error = false;
            apiRep.Data = stocks;
            return Ok(apiRep);
        }

    }
}