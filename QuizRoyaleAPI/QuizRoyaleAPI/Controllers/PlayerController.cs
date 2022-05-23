using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizRoyaleAPI.DTOs;
using QuizRoyaleAPI.Models;
using QuizRoyaleAPI.Services;
using QuizRoyaleAPI.Services.Auth;
using System.Security.Claims;

namespace QuizRoyaleAPI.Controllers
{
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
            string token = _authService.GetToken(id);
            return Ok(new TokenDTO(token));
        }

        [HttpDelete]
        public IActionResult DeletePlayer()
        {
            _playerService.DeletePlayer(1);
            return Ok();
        }
    }
}
