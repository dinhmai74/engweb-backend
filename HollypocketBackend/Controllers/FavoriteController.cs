using System.Security.Claims;
using HollypocketBackend.Models;
using HollypocketBackend.Services;
using HollypocketBackend.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;

namespace HollypocketBackend.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        private readonly FavoriteService _favoriteService;
        public FavoriteController(FavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }
        [HttpPost("InsertProductId")]
        public IActionResult Insert(string productId)
        {
            var apiRep = new APIResponse();
            var userId = string.Empty;
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                userId = identity.FindFirst(ClaimTypes.Name).Value;
            }

            List<Favorite> ListFavorite = _favoriteService.Get();
            if (ListFavorite == null)
            {
                apiRep.Error = true;
                apiRep.Message = "Error";
                return BadRequest(apiRep);
            }
            var flag = 0;
            foreach (Favorite favorite in ListFavorite)
            {
                if (favorite.UserId == userId)
                {
                    flag = 1;
                    break;
                }
            }
            if (flag == 0)
            {
                String[] ProducdIdCreate = new[] { productId };
                var F = new Favorite
                {
                    UserId = userId,
                    ProductId = ProducdIdCreate
                };
                _favoriteService.Insert(F);
                apiRep.Error = false;
                apiRep.Data = F;
                return Ok(apiRep);
            }
            else
            {
                foreach (Favorite item in ListFavorite)
                {
                    if (item.UserId == userId)
                    {
                        var F = item;
                        for (var i = 0; i < item.ProductId.Length; i++)
                        {
                            if (F.ProductId[i] == productId)
                            {
                                apiRep.Message = "Error";
                                apiRep.Data = item;
                                return Ok(apiRep);
                            }
                        }
                        String[] ProductIdUpdate = new string[F.ProductId.Length + 1];
                        for (var j = 0; j < F.ProductId.Length + 1; j++)
                        {
                            if (j < 0)
                            {
                                ProductIdUpdate[j] = F.ProductId[j];

                            }
                            else if (j == 0)
                            {
                                ProductIdUpdate[j] = productId;
                            }
                            else
                            {
                                ProductIdUpdate[j] = F.ProductId[j - 1];
                            }
                        }
                        F.ProductId = ProductIdUpdate;
                        _favoriteService.Update(F.Id, F);
                        apiRep.Data = F;
                        break;

                    }
                }
            }

            return Ok(apiRep);
        }
        [HttpGet("get-all")]
        public ActionResult Get()
        {
            var items = _favoriteService.Get();
            var apiRep = new APIResponse();
            apiRep.Data = items;
            return Ok(apiRep);
        }

        [HttpGet("GetId")]
        public ActionResult<Favorite> GetById()
        {
            var apiRep = new APIResponse();
            var userId = string.Empty;
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                userId = identity.FindFirst(ClaimTypes.Name).Value;
            }
            var favorite = _favoriteService.GetById(userId);
            apiRep.Error = false;
            apiRep.Data = favorite;
            return Ok(apiRep);
        }
        [HttpDelete("Delete")]
        public IActionResult Delete(string id)
        {
            var Favorite = _favoriteService.Get(id);
            if (Favorite == null)
            {
                return NotFound();
            }
            _favoriteService.Delete(Favorite.Id);
            return NoContent();
        }
        [HttpDelete("DeleteProduct")]
        public ActionResult DeleteProduct(string productId)
        {
            var apiRep = new APIResponse();
            List<Favorite> ListFavorite = _favoriteService.Get();
            var userId = string.Empty;
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                userId = identity.FindFirst(ClaimTypes.Name).Value;
            }
            foreach (Favorite item in ListFavorite)
            {
                if (item.UserId == userId)
                {
                    for (var i = 0; i < item.ProductId.Length; i++)
                    {
                        if (productId == item.ProductId[i])
                        {
                            var j = 0;
                            var k = 0;
                            String[] FavoriteUpdate = new String[item.ProductId.Length - 1];
                            while (j < item.ProductId.Length)
                            {
                                if (j != i)
                                {
                                    FavoriteUpdate[k] = item.ProductId[j];
                                    k++;
                                }
                                j++;
                            }
                            item.ProductId = FavoriteUpdate;
                            _favoriteService.Update(item.Id, item);
                            apiRep.Data = item;
                        }
                    }
                }
            }
            return Ok(apiRep);
        }
    }
}
