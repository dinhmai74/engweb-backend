using System.Security.Claims;
using HollypocketBackend.Models;
using HollypocketBackend.Services;
using HollypocketBackend.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Configuration.UserSecrets;
using HollypocketBackend.Models.Product;

namespace HollypocketBackend.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    [CustomExceptionFilter]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly CartService _cartService;
        private readonly ProductService _productService;
        public OrderController(OrderService orderService, CartService cartService, ProductService productService)
        {
            _orderService = orderService;
            _cartService = cartService;
            _productService = productService;
        }
        [HttpPost("UpdateStatus")]
        public ActionResult UpdateStatus(string id, string status)
        {
            var apiRep = new APIResponse();
            List<Order> Orders = _orderService.Get();
            if (Orders == null)
            {
                apiRep.Error = true;
                apiRep.Message = "Can't not find list Order";
                return BadRequest(apiRep);
            }
            foreach (Order item in Orders)
            {
                if (item.Id == id)
                {
                    item.orderinfo.status = status;
                    apiRep.Error = false;
                    apiRep.Data = item;
                    _orderService.Update(item.Id, item);
                }
            }
            return Ok(apiRep);
        }
        [HttpPost("Create")]
        public ActionResult Create(OrderInput dto)

        {
            var apiRep = new APIResponse();
            var userId = string.Empty;
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                userId = identity.FindFirst(ClaimTypes.Name).Value;
            }

            List<Cart> cartss = _cartService.Get();
            if (cartss == null)
            {
                throw new Exception("Can't not find cart");
            }

            foreach (Cart c in cartss)
            {
                if (c.UserId == userId)
                {
                    List<Pro> Pros = new List<Pro>();
                    for (var index = 0; index < c.Products.Length; index++)
                    {
                        Pro pro = new Pro();
                        Product product = new Product();
                        product = _productService.GetId(c.Products[index].ProductId);
                        pro.name = product.ProductName;
                        pro.provider = product.Provider;
                        pro.price = product.Price;
                        pro.Amount = c.Products[index].Amount;
                        pro.ProductId = c.Products[index].ProductId;
                        Pros.Add(pro);
                    }
                    var oi = new Orderinfo
                    {
                        carts = Pros.ToArray(),
                        CashType = dto.CashType,
                        ReceiverAdderss = dto.ReceiverAddress,
                        OrderDate = dto.OrderDate,
                        Phone = dto.Phone,
                        Name = dto.Name,

                        Note = dto.Note,
                        status = "Unconfimred"
                    };
                    var Order = new Order
                    {
                        AccountId = userId,
                        orderinfo = oi
                    };

                    _orderService.Insert(Order);
                    _cartService.Delete(c);
                    apiRep.Error = false;
                    apiRep.Data = Order;
                }
            }
            return Ok(apiRep);
        }
        [HttpGet("get-all")]
        public ActionResult<List<Order>> Get() => _orderService.Get();
        [HttpGet("GetById")]
        public ActionResult<List<Order>> GetById(string userId)
        {
            var apiRep = new APIResponse();
            List<Order> UserOrder = _orderService.GetById(userId);
            apiRep.Error = false;
            apiRep.Data = UserOrder;
            return Ok(apiRep);

        }
        [HttpGet("get-list-orders")]
        public ActionResult<List<Order>> GetById()
        {
            var apiRep = new APIResponse();
            var userId = string.Empty;
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                userId = identity.FindFirst(ClaimTypes.Name).Value;
            }
            List<Order> UserOrder = _orderService.GetById(userId);
            apiRep.Error = false;
            apiRep.Data = UserOrder;
            return Ok(apiRep);

        }
        [HttpGet("get-list-bought-products")]

        public IActionResult GetAccountOrder()
        {
            AccountOrder accountOrder = new AccountOrder();
            var apiRep = new APIResponse();
            var userId = string.Empty;
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                userId = identity.FindFirst(ClaimTypes.Name).Value;
            }
            accountOrder.AccountId = userId;
            List<Order> Orders = _orderService.Get();
            if (Orders == null)
            {
                apiRep.Error = true;
                apiRep.Message = "Can't not find list order";
                return BadRequest(apiRep);
            }
            foreach (Order item in Orders)
            {
                if (item.AccountId == accountOrder.AccountId)
                {
                    for (var i = 0; i < item.orderinfo.carts.Length; i++)
                    {
                        var flag = 0;
                        var index = 0;
                        for (var j = 0; j < accountOrder.carts.Count; j++)
                        {

                            for (var k = 0; k < accountOrder.carts.Count; k++)
                            {
                                if (accountOrder.carts[j].ProductId.ToString() == item.orderinfo.carts[i].ProductId)
                                {
                                    flag = 1;
                                    index = j;
                                    break;
                                }
                            }
                        }
                        if (flag == 0)
                        {
                            Pro pro = new Pro();
                            pro = item.orderinfo.carts[i];
                            accountOrder.carts.Add(pro);
                        }
                        else
                        {
                            accountOrder.carts[index].Amount += item.orderinfo.carts[i].Amount;
                        }
                    }
                    apiRep.Data = accountOrder;

                }
            }
            return Ok(apiRep);
        }
        [HttpDelete("Delete")]
        public IActionResult Delete(string id)
        {
            var Order = _orderService.Get(id);

            if (Order == null)
            {
                return NotFound();
            }

            _cartService.Delete(Order.Id);

            return NoContent();
        }
    }
}
