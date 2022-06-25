using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizRoyaleAPI.DTOs;
using QuizRoyaleAPI.Services.Auth;
using QuizRoyaleAPI.Services.Data;

namespace QuizRoyaleAPI.Controllers
{
    /// <summary>
    /// Player Controller, Meer informatie over de player endpoints zijn te vinden in swagger.
    /// Deze klasse handelt het creëren van PlayerDTO's af.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class PlayerController : Controller
    {
        private readonly IPlayerService _playerService;
        private readonly IAuthService _authService;

        public PlayerController(IPlayerService playerService, IAuthService authService)
        {
            _playerService = playerService;
            _authService = authService;
        }

        [HttpGet]
        public IActionResult GetPlayer()
        {
            return Ok(_playerService.GetPlayer(User.GetID()));
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreatePlayer([FromBody] PlayerCreationDTO player)
        {
            int id = _playerService.CreatePlayer(player.Username);
            return Ok(_authService.GetToken(id));
        }

        [HttpDelete]
        public IActionResult DeletePlayer()
        {
            _playerService.DeletePlayer(User.GetID());
            return Ok();
        }
    }
}
