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
    [Authorize]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly QuestionService _questionService;
        private readonly AccountService _accountService;
        public QuestionController(QuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpGet]
        public ActionResult<List<Question>> Get() =>
        _questionService.Get();

        [AllowAnonymous]
        [HttpGet("get-question/{productId}")]
        public ActionResult<Question> Get(string productId, int pageSize = 10, int pageNumber = 1)
        {

            var apiRep = new APIResponse();
            var userid = string.Empty;
            if (HttpContext.User.Identity is ClaimsIdentity identity)
                userid = identity.FindFirst(ClaimTypes.Name).Value;

            var question = _questionService.GetWithPage(productId, pageSize, pageNumber);

            apiRep.Error = false;
            apiRep.Data = question;
            if (question == null)
            {
                apiRep.Error = true;
                apiRep.Data = null;
                return Ok(apiRep);
            }

            return Ok(apiRep);
        }

        [HttpPost("answer-Question")]
        public ActionResult<Question> answer(string answer, string questionId)
        {
            var apiRep = new APIResponse();
            var UserId = string.Empty;
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                UserId = identity.FindFirst(ClaimTypes.Name).Value;
            }
            var question = _questionService.AnswerQuestion(answer, questionId);
            apiRep.Error = false;
            apiRep.Data = question;

            return Ok(apiRep);
        }
        [HttpPost("insert-Question")]
        public ActionResult<Question> Create(QuestionDto question)
        {
            var apiRep = new APIResponse();
            var UserId = string.Empty;
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                UserId = identity.FindFirst(ClaimTypes.Name).Value;
            }
            _questionService.Create(question, UserId);

            apiRep.Error = false;
            apiRep.Data = question;

            return Ok(apiRep);
        }


        [HttpPut("update-Question/{id}")]
        public IActionResult Update(string id, Question questionIn)
        {
            var question = _questionService.Get(id);

            if (question == null)
            {
                return NotFound();
            }

            _questionService.Update(id, questionIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var question = _questionService.Get(id);

            if (question == null)
            {
                return NotFound();
            }

            _questionService.Delete(question.productId);

            return NoContent();
        }

    }
}
