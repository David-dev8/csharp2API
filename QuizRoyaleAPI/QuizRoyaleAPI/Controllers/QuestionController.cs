using Microsoft.AspNetCore.Mvc;

namespace QuizRoyaleAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
