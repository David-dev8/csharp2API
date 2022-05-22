using Microsoft.AspNetCore.Mvc;
using QuizRoyaleAPI.Models;
using QuizRoyaleAPI.Services;

namespace QuizRoyaleAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : Controller
    {
        private readonly IQuestionService _questionService;

        public PlayerController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpGet]
        public IActionResult GetPlayers()
        {
            return Ok(_questionService.getQuestions());
        }
    }
}
