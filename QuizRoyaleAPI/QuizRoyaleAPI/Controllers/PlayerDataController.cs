using Microsoft.AspNetCore.Mvc;
using QuizRoyaleAPI.Services;

namespace QuizRoyaleAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class PlayerDataController : Controller
    {
        private readonly IPlayerDataService _playerDataService; // todo validation post

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
            return Ok();
        }

        [HttpGet("Result")]
        public IActionResult GetResults()
        {
            return Ok(_playerDataService.GetResults(1));
        }

        [HttpGet("Rank")]
        public IActionResult GetRank()
        {
            return Ok(_playerDataService.GetRank(1));
        }
    }
}
