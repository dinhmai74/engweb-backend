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
    [Authorize]
    [ApiController]
    [CustomExceptionFilter]
    public class AccountsController : ControllerBase
    {
        private readonly AccountService _accountService;

        public AccountsController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("sign-in")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            var apiRep = new APIResponse();
            var user = _accountService.Authenticate(model.Email, model.Password);

            if (user == null) throw new Exception("Username or password is incorrect");
            apiRep.Data = new
            {
                Info = ConvertAccountToDTO(user),
                Token = user.Token
            };

            return Ok(apiRep);
        }

        [HttpGet("get-all")]
        public ActionResult Get()
        {
            var userId = string.Empty;
            if (HttpContext.User.Identity is ClaimsIdentity identity)
                userId = identity.FindFirst(ClaimTypes.Name).Value;

            var user = _accountService.Get(userId);
            if (user == null || user.AccountType != 0) throw new Exception("Can't access");

            var apiRep = new APIResponse();
            apiRep.Data = _accountService.Get().FindAll(x => true).WithoutPasswords();
            return Ok(apiRep);
        }

        [HttpGet("get-user-info")]
        public IActionResult GetUserInfo()
        {
            var apiRep = new APIResponse();
            var userId = string.Empty;
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                userId = identity.FindFirst(ClaimTypes.Name).Value;
            }

            var errMess = StringHelper.Instance.APIInvalidToken;
            if (userId == null) throw new Exception(errMess);


            var user = _accountService.Get(userId);
            if (user == null) throw new Exception(errMess);

            apiRep.Data = ConvertAccountToDTO(user);
            return Ok(apiRep);
        }

        [HttpPost("add-address")]
        public IActionResult AddAddress([FromBody] Address model)
        {
            var apiRep = new APIResponse();
            if (model.Id == null) model.Id = Guid.NewGuid().ToString();

            var userId = string.Empty;
            if (HttpContext.User.Identity is ClaimsIdentity identity)
                userId = identity.FindFirst(ClaimTypes.Name).Value;

            if (userId == null) return BadRequest(new { Error = true, Message = StringHelper.Instance.APIInvalidToken });
            var Account = _accountService.Get(userId);
            if (Account == null) return BadRequest(new { Error = true, Message = StringHelper.Instance.APIInvalidToken });

            Account.Addresses.Add(model);
            _accountService.Update(userId, Account);

            apiRep.Message = StringHelper.Instance.APIUpdateSuccess;
            apiRep.Data = ConvertAccountToDTO(Account);

            return Ok(apiRep);
        }

        [HttpPost("edit-address")]
        public IActionResult EditAddress([FromBody] Address model)
        {
            var apiRep = new APIResponse();

            var userId = string.Empty;
            if (HttpContext.User.Identity is ClaimsIdentity identity)
                userId = identity.FindFirst(ClaimTypes.Name).Value;

            if (userId == null) return BadRequest(new { Error = true, Message = StringHelper.Instance.APIInvalidToken });
            var Account = _accountService.Get(userId);
            if (Account == null) return BadRequest(new { Error = true, Message = StringHelper.Instance.APIInvalidToken });

            var index = Account.Addresses.FindIndex(x => x.Id == model.Id);
            if (index >= 0)
            {
                Account.Addresses[index] = model;
                _accountService.Update(userId, Account);
                apiRep.Message = StringHelper.Instance.APIUpdateSuccess;
                apiRep.Data = ConvertAccountToDTO(Account);
            }
            else
            {
                apiRep.Error = true;
                apiRep.Message = "Can't find the address, please try again later!";
            }

            return Ok(apiRep);
        }

        [HttpPost("delete-address")]
        public IActionResult Delete([FromBody] Address model)
        {
            var apiRep = new APIResponse();

            var userId = string.Empty;
            if (HttpContext.User.Identity is ClaimsIdentity identity)
                userId = identity.FindFirst(ClaimTypes.Name).Value;

            if (userId == null) return BadRequest(new { Error = true, Message = StringHelper.Instance.APIInvalidToken });
            var Account = _accountService.Get(userId);
            if (Account == null) return BadRequest(new { Error = true, Message = StringHelper.Instance.APIInvalidToken });

            var index = Account.Addresses.FindIndex(x => x.Id == model.Id);
            if (index >= 0)
            {
                Account.Addresses.RemoveAt(index);
                _accountService.Update(userId, Account);
                apiRep.Message = StringHelper.Instance.APIUpdateSuccess;
                apiRep.Data = ConvertAccountToDTO(Account);
            }
            else
            {
                apiRep.Error = true;
                apiRep.Message = "Can't find the address, please try again later!";
            }

            return Ok(apiRep);
        }


        [AllowAnonymous]
        [HttpPost("sign-up")]
        public ActionResult CreateUser([FromBody] SignUpModel model)
        {
            var apiRep = new APIResponse();
            /* ------------------------ validate ------------------------ */
            var rs = StringHelper.Instance.IsEmailString(model.Email);
            if (!rs) throw new Exception("Invalid email!");

            var existAccount = _accountService.GetByEmail(model.Email);
            if (existAccount != null) throw new Exception("There have an existing account!");

            /* ------------------------ insert db ------------------------ */
            var account = new Account
            {
                Email = model.Email,
                Name = model.Name,
                Password = model.Password,
                Addresses = new List<Address>(),
                AccountType = AccountType.Normal
            };
            _accountService.Insert(account);
            var user = _accountService.Authenticate(model.Email, model.Password);

            // return CreatedAtRoute("GetAccount", new { id = model.Id.ToString() }, ConvertAccountToDTO(account));
            apiRep.Error = false;
            apiRep.Data = new
            {
                info = ConvertAccountToDTO(user),
                token = user.Token
            };

            return Ok(apiRep);
        }

        [HttpGet("logout")]
        public ActionResult Logout()
        {
            var apiRep = new APIResponse();
            var userId = string.Empty;
            if (HttpContext.User.Identity is ClaimsIdentity identity)
                userId = identity.FindFirst(ClaimTypes.Name).Value;

            if (userId == null) return BadRequest(new { Error = true, Message = StringHelper.Instance.APIInvalidToken });
            var Account = _accountService.Get(userId);
            if (Account == null) return BadRequest(new { Error = true, Message = StringHelper.Instance.APIInvalidToken });

            Account.Token = null;
            _accountService.Update(userId, Account);
            apiRep.Message = "Logout success!";

            return Ok(apiRep);
        }

        [HttpPost("update-info-with-password")]
        public IActionResult UpdateAll([FromBody] EditUserInfoModel model)
        {
            var apiRep = new APIResponse();
            var userId = string.Empty;
            if (HttpContext.User.Identity is ClaimsIdentity identity)
                userId = identity.FindFirst(ClaimTypes.Name).Value;

            if (userId == null) return BadRequest(new { Error = true, Message = StringHelper.Instance.APIInvalidToken });
            var Account = _accountService.Get(userId);
            if (Account == null) return BadRequest(new { Error = true, Message = StringHelper.Instance.APIInvalidToken });

            var result = _accountService.CheckValidPassword(Account.Email, model.OldPassword);

            if (!result) return Ok(new { Error = true, Message = "Your old password isn't correct!" });

            Account.Password = model.NewPassword;
            Account.Name = model.Name;
            Account.PhoneNumber = model.PhoneNumber;
            Account.Gender = model.Gender;

            _accountService.Update(userId, Account);
            apiRep.Data = new
            {
                Gender = Account.Gender,
                Name = Account.Name,
                PhoneNumber = Account.PhoneNumber,
            };

            apiRep.Message = "Update successfull!";

            return Ok(apiRep);
        }

        [HttpPost("update-info")]
        public IActionResult Update(AccountDto AccountIn)
        {
            var apiRep = new APIResponse();
            var userId = string.Empty;
            if (HttpContext.User.Identity is ClaimsIdentity identity)
                userId = identity.FindFirst(ClaimTypes.Name).Value;

            if (userId == null) return BadRequest(new { Error = true, Message = "Invalid token!" });
            var Account = _accountService.Get(userId);
            if (Account == null) return BadRequest(new { Error = true, Message = "Invalid token!" });

            Account.Name = AccountIn.Name;
            Account.PhoneNumber = AccountIn.PhoneNumber;
            Account.Gender = AccountIn.Gender;
            _accountService.Update(userId, Account);
            apiRep.Data = new
            {
                Gender = Account.Gender,
                Name = Account.Name,
                PhoneNumber = Account.PhoneNumber,
            };
            apiRep.Message = "Update successfull!";

            return Ok(apiRep);
        }

        [HttpPost("update-password")]
        public IActionResult UpdatePassword([FromBody] AccoutPasswordModel model)
        {
            var apiRep = new APIResponse();
            var userId = string.Empty;
            if (HttpContext.User.Identity is ClaimsIdentity identity)
                userId = identity.FindFirst(ClaimTypes.Name).Value;

            if (userId == null) return BadRequest(new { Error = true, Message = StringHelper.Instance.APIInvalidToken });
            var Account = _accountService.Get(userId);
            if (Account == null) return BadRequest(new { Error = true, Message = StringHelper.Instance.APIInvalidToken });

            var result = _accountService.CheckValidPassword(Account.Email, model.OldPassword);

            if (!result) return Ok(new { Error = true, Message = "Your old password isn't correct!" });

            Account.Password = model.NewPassword;
            _accountService.Update(userId, Account);
            apiRep.Data = "";
            apiRep.Message = "Update successfull!";

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
