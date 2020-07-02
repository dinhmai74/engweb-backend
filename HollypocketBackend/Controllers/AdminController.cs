using System.Security.Claims;
using HollypocketBackend.Models;
using HollypocketBackend.Services;
using HollypocketBackend.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System;

namespace HollypocketBackend.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    [CustomExceptionFilter]
    public class AdminController : ControllerBase
    {
        private readonly AccountService _accountService;

        public AdminController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("get-all")]
        public ActionResult Get()
        {

            var apiRep = new APIResponse();
            apiRep.Data = _accountService.Get().FindAll(x => true).WithoutPasswords();
            return Ok(apiRep);
        }

        [AllowAnonymous]
        [HttpPost("sign-in")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            var apiRep = new APIResponse();
            var user = _accountService.Authenticate(model.Email, model.Password);

            if (user == null || user.AccountType != AccountType.Admin) throw new Exception("Username or password is incorrect");
            apiRep.Data = new
            {
                Info = ConvertAccountToDTO(user),
                Token = user.Token
            };

            return Ok(apiRep);
        }

        [HttpPost("delete/{id}")]
        public IActionResult Delete(string id)
        {
            var apiRep = new APIResponse();
            var Account = _accountService.Get(id);

            if (Account == null)
            {
                apiRep.Error = true;
                apiRep.Message = "Invalid id!";
                return NotFound(apiRep);
            }

            _accountService.Delete(Account.Id);
            apiRep.Message = "Deleted account!";
            apiRep.Data = ConvertAccountToDTO(Account);
            return Ok(apiRep);
        }


        private static AccountDto ConvertAccountToDTO(Account acc) =>
               new AccountDto
               {
                   Name = acc.Name,
                   Email = acc.Email,
                   PhoneNumber = acc.PhoneNumber,
                   Gender = acc.Gender,
                   Addresses = acc.Addresses,
                   Id = acc.Id,
                   AccountType = acc.AccountType

               };
    }
}