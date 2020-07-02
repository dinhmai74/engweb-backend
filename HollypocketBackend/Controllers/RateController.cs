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
    public class RateController : ControllerBase
    {
        private readonly RateService _rateService;
        private readonly AccountService _accountService;
        public RateController(RateService rateService)
        {
            _rateService = rateService;
        }

        [HttpGet]
        public ActionResult<List<Rate>> Get() =>
        _rateService.Get();

        [HttpGet("get-rate-byId/{id}")]
        public ActionResult Get(string id)
        {
            var apiRep = new APIResponse();
            var rate = _rateService.Get(id);
            apiRep.Error = false;
            if (rate == null)
            {
                apiRep.Error = true;
                apiRep.Data = null;
                return Ok(apiRep);
            }
            apiRep.Error = false;
            apiRep.Data = rate;

            return Ok(apiRep);
        }

        [HttpGet("get-averagerate-byId/{productId}")]
        public ActionResult GetAverage(string productId)
        {
            var apiRep = new APIResponse();
            var average = _rateService.Average(productId);
            apiRep.Error = false;
            
            apiRep.Error = false;
            apiRep.Data =new {average};

            return Ok(apiRep);
        }
        [HttpPost("insert-Rate")]
        public ActionResult<Rate> Create(RateInfo rateInfo, string productId)
        {
            var apiRep = new APIResponse();
            // var UserId = string.Empty;
            // if (HttpContext.User.Identity is ClaimsIdentity identity)
            // {
            //     UserId = identity.FindFirst(ClaimTypes.Name).Value;
            // }
            var _rate = new Rate
            {
                userId = "2",
                productId = productId,
                rate = rateInfo
            };
            var rate = _rateService.Create(_rate);

            apiRep.Error = false;
            apiRep.Data = rate;

            return Ok(apiRep);
        }

        [HttpPut("update-Rate/{id}")]
        public IActionResult Update(string id, Rate rateIn)
        {
            var rate = _rateService.Get(id);

            if (rate == null)
            {
                return NotFound();
            }

            _rateService.Update(id, rateIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var apiRep = new APIResponse();
            var rate = _rateService.Get(id);

            if (rate == null)
            {
                apiRep.Error = false;
                apiRep.Data = null;
                return Ok(apiRep);
            }

            _rateService.Delete(rate.Id);

            apiRep.Error = false;
            apiRep.Data = true;
            return Ok(apiRep);
        }

    }
}
