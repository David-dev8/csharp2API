using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizRoyaleAPI.Services.Data;

namespace QuizRoyaleAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class PlayerDataController : Controller
    {
        private readonly IPlayerDataService _playerDataService;

        public PlayerDataController(IPlayerDataService playerDataService)
        {
            _playerDataService = playerDataService;
        }

        [HttpGet("Mastery")]
        public IActionResult GetMastery()
        {
            return Ok(_playerDataService.GetMastery(User.GetID()));
        }

        [HttpGet("Badges")]
        public IActionResult GetBadges()
        {
            return Ok(_playerDataService.GetBadges(User.GetID()));
        }

        [HttpGet("Result")]
        public IActionResult GetResults()
        {
            return Ok(_playerDataService.GetResults(User.GetID()));
        }

        [HttpGet("Rank")]
        public IActionResult GetRank()
        {
            return Ok(_playerDataService.GetDivision(User.GetID()));
        }
    }
}
