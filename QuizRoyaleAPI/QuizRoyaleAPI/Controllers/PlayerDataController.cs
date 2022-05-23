using Microsoft.AspNetCore.Mvc;
using QuizRoyaleAPI.Services;

namespace QuizRoyaleAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerDataController : Controller
    {
        private readonly IPlayerDataService _playerDataService;

        public PlayerDataController(IPlayerDataService playerDataService)
        {
            _playerDataService = playerDataService;
        }

        [HttpGet("mastery")]
        public IActionResult GetMastery()
        {
            return Ok(_playerDataService.GetMastery(1));
        }

        [HttpGet("badges")]
        public IActionResult GetBadges()
        {
            return Ok();
        }

        [HttpGet("result")]
        public IActionResult GetResults()
        {
            return Ok(_playerDataService.GetResults(1));
        }

        [HttpGet("rank")]
        public IActionResult GetRank()
        {
            return Ok(_playerDataService.GetRank(1));
        }
    }
}
