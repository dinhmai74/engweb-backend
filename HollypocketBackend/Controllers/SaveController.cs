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
    public class SaveController : ControllerBase
    {
        private readonly SaveService _saveService;
        public SaveController(SaveService save)
        {
            _saveService = save;
        }
        [HttpPost("Create")]
        public IActionResult Insert(S p)
        {
            var apiRep = new APIResponse();
            var userId = string.Empty;
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                userId = identity.FindFirst(ClaimTypes.Name).Value;
            }
            var Save = new Save
            {
                UserId = userId,
                Product = p
            };
            _saveService.Insert(Save);
            apiRep.Error = false;
            apiRep.Data = Save;
            return Ok(apiRep);

        }
        [HttpDelete("Delete")]
        public IActionResult Delete(string id)
        {
            var Save = _saveService.Get(id);
            if (Save == null)
            {
                return NotFound();
            }
            _saveService.Delete(Save.Id);
            return NoContent();
        }
    }
}
