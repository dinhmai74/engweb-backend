using HollypocketBackend.Models;
using HollypocketBackend.Models.Provider;
using HollypocketBackend.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HollypocketBackend.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProvidersController : ControllerBase
    {
        private readonly ProviderService _providerService;

        public ProvidersController(ProviderService providerService)
        {
            _providerService = providerService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var apiRep = new APIResponse();
            var providers = await _providerService.GetAll();
            apiRep.Error = false;
            apiRep.Data = providers;
            return Ok(apiRep);
        }

        [HttpGet("{providerId}")]
        public async Task<ActionResult> GetById(string providerId)
        {

            var apiRep = new APIResponse();
            var product = await _providerService.GetById(providerId);

            apiRep.Error = false;
            apiRep.Data = product;

            return Ok(apiRep);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateProviderModel provider)
        {

            var apiRep = new APIResponse();
            apiRep.Error = false;
            apiRep.Data = await _providerService.Create(provider);

            return Ok(apiRep);
        }

        [HttpPut("{providerId}")]
        public async Task<IActionResult> Update(string providerId, UpdateProviderModel provider)
        {
            var apiRep = new APIResponse();
            if (!(await _providerService.IsExisting(providerId)))
            {
                apiRep.Data = null;
                apiRep.Error = false;

                return Ok(apiRep);
            }
            var p = await _providerService.Update(providerId, provider);
            apiRep.Data = p;
            apiRep.Error = false;
            return Ok(apiRep);
        }

        [HttpDelete("{providerId}")]
        public async Task<IActionResult> Delete(string providerId)
        {
            var apiRep = new APIResponse();
            if (!(await _providerService.IsExisting(providerId)))
            {
                apiRep.Data = null;
                apiRep.Error = false;

                return Ok(apiRep);
            }

            var p = await _providerService.Delete(providerId);

            apiRep.Data = p;
            apiRep.Error = false;

            return Ok(apiRep);
        }


        [HttpGet("generate-mock-data/{times}")]
        public async Task<IActionResult> GenerateMockData(Int64 times)
        {
            var apiRep = new APIResponse();
            string[] providerNames = { "Wolsen", "HuonGoo", " I’m Game", "Rongyuxuan", "Sony", "Nvidia Shield", "LeapFrog", "Nintendo" };

            for (var i = 0; i < providerNames.Length; i++)
            {
                var provider = new CreateProviderModel
                {
                    Name = providerNames[i],
                    Description = ""
                };
                await _providerService.Create(provider);
            }

            apiRep.Error = false;
            apiRep.Data = true;
            return Ok(apiRep);
        }
    }
}
