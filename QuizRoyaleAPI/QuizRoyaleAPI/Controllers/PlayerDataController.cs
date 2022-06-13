using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizRoyaleAPI.Services.Data;

namespace QuizRoyaleAPI.Controllers
{
    /// <summary>
    /// Player database Controller, Meer informatie over de player endpoints zijn te vinden in swagger
    /// Deze klasse handeld het ophalen van extra data van een player af
    /// </summary>
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
