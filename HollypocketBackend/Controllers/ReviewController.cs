using HollypocketBackend.Models;
using HollypocketBackend.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HollypocketBackend.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReviewControler : ControllerBase
    {
        private readonly ReviewService _reviewService;
        private readonly AccountService _accountService;
        public ReviewControler(ReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public ActionResult<List<Review>> Get() =>
        _reviewService.Get();

        [HttpGet("get-review/{id}")]
        public ActionResult<Review> Get(string productId)
        {

            var apiRep = new APIResponse();
            var userid = string.Empty;
            if (HttpContext.User.Identity is ClaimsIdentity identity)
                userid = identity.FindFirst(ClaimTypes.Name).Value;

            if (userid == null) return BadRequest(new { Error = true, Message = "Invalid token!" });
            var Account = _accountService.Get(userid);
            if (Account == null) return BadRequest(new { Error = true, Message = "Invalid token!" });

            var review = _reviewService.Get(productId, userid);

            apiRep.Error = false;
            apiRep.Data = review;
            if (review == null)
            {
                apiRep.Error = true;
                apiRep.Data = null;
                return Ok(apiRep);
            }

            return Ok(apiRep);
        }

        [HttpPost("insert-Review")]
        public ActionResult<Review> Create(ReviewDTO review)
        {
            var apiRep = new APIResponse();
            var UserId = string.Empty;
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                UserId = identity.FindFirst(ClaimTypes.Name).Value;
            }
            var _review = new Review
            {
                userId = UserId,
                value = review.value,

            };
            _reviewService.Create(_review);

            apiRep.Error = false;
            apiRep.Data = review;

            return Ok(apiRep);
        }

    }
}
