using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuizRoyaleAPI.Exceptions;

namespace QuizRoyaleAPI.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : Controller
    {
        [Route("/error")]
        public IActionResult ProcessUnhandledErrors()
        {
            var error = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

            int status = error switch
            {
                PlayerNotFoundException or ItemNotFoundException => StatusCodes.Status404NotFound,
                UsernameTakenException or InsufficientFundsException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            return Problem(statusCode: status, detail: error?.Message);
        }
    }
}
