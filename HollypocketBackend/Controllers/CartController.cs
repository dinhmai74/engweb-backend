using System.Security.Claims;
using HollypocketBackend.Models;
using HollypocketBackend.Services;
using HollypocketBackend.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace HollypocketBackend.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;
        public CartController(CartService cartService)
        {
            _cartService = cartService;

        }

        [HttpPost("add-products")]
        public ActionResult Create(Carts dto)

        {
            var apiRep = new APIResponse();
            var userId = string.Empty;
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                userId = identity.FindFirst(ClaimTypes.Name).Value;
            }

            var Cart = new Cart
            {
                UserId = userId,
                Products = dto.product
            };
            _cartService.Insert(Cart);
            apiRep.Error = false;
            apiRep.Data = Cart;
            return Ok(apiRep);
        }
        [HttpPost("Update-Amount-in-Cart")]
        public ActionResult UpdateAmount(string productId, int amount)
        {
            var apiRep = new APIResponse();
            var userId = string.Empty;
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                userId = identity.FindFirst(ClaimTypes.Name).Value;
            }
            var Cart = _cartService.GetUserId(userId);
            for(var i=0;i<Cart.Products.Length;i++)
            {
                if(Cart.Products[i].ProductId==productId)
                {
                    Cart.Products[i].Amount = amount;
                    apiRep.Error = false;
                    apiRep.Data = Cart;
                    _cartService.Update(Cart.Id, Cart);
                    break;
                }
            }
            return Ok(apiRep);
        }
        [HttpPost("add-product")]
        public ActionResult AddProduct(P dto)

        {
            var apiRep = new APIResponse();
            var userId = string.Empty;
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                userId = identity.FindFirst(ClaimTypes.Name).Value;
            }
            
            P[] products = { dto };
            var Cart = new Cart
            {
                UserId = userId,
                Products = products
            };
            _cartService.Insert(Cart);
            apiRep.Error = false;
            apiRep.Data = Cart;
            return Ok(apiRep);
        }
        [HttpPost("update")]
        public IActionResult Update(string id, Carts cartIn)
        {
            var apiRep = new APIResponse();
            var Cart = _cartService.Get(id);

            if (Cart == null)
            {
                apiRep.Error = true;
                apiRep.Message = "Invalid id!";
                return NotFound(apiRep);
            }

            Cart.Products = cartIn.product;
            _cartService.Update(id, Cart);
            apiRep.Data = cartIn;

            return Ok(apiRep);
        }
        [HttpGet("get-all")]
        public ActionResult GetCarts()
        {
            var userId = string.Empty;
            if (HttpContext.User.Identity is ClaimsIdentity identity) { userId = identity.FindFirst(ClaimTypes.Name).Value; }

            var cart = _cartService.GetUserId(userId);
            var apiRep = new APIResponse();
            apiRep.Data = cart;

            return Ok(apiRep);
        }
        [HttpGet("count-items")]
        public ActionResult GetCurrentItems()
        {
            var userId = string.Empty;
            if (HttpContext.User.Identity is ClaimsIdentity identity) { userId = identity.FindFirst(ClaimTypes.Name).Value; }

            var cart = _cartService.GetUserId(userId);
            var apiRep = new APIResponse();
            if (cart != null)
                apiRep.Data = cart.Products.Length;
            else apiRep.Data = 0;

            return Ok(apiRep);
        }


        [HttpDelete("Delete")]
        public IActionResult Delete(string id)
        {
            var Cart = _cartService.Get(id);

            if (Cart == null)
            {
                return NotFound();
            }

            _cartService.Delete(Cart.Id);

            return NoContent();
        }
        [HttpDelete("DeleteProduct")]
        public ActionResult DeleteProduct(string productID)
        {
            var apiRep = new APIResponse();
            List<Cart> ListCart = _cartService.Get();
            var userId = string.Empty;
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                userId = identity.FindFirst(ClaimTypes.Name).Value;
            }
            foreach (Cart item in ListCart)
            {

                if (item.UserId == userId)
                {
                    for (var i = 0; i < item.Products.Length; i++)
                    {
                        if (productID == item.Products[i].ProductId)
                        {
                            var j = 0;
                            var k = 0;
                            P[] cartsUpdate = new P[item.Products.Length - 1];
                            while (j < item.Products.Length)
                            {
                                if (j != i)
                                {
                                    cartsUpdate[k] = item.Products[j];
                                    k++;
                                }

                                j++;
                            }
                            item.Products = cartsUpdate;
                            _cartService.Update(item.Id, item);
                            apiRep.Data = item;

                        }
                    }



                }
            }


            return Ok(apiRep);
        }
    }
}
