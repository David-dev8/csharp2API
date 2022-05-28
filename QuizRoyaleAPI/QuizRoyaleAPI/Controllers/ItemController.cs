using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizRoyaleAPI.Services.Data;

namespace QuizRoyaleAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ItemController : Controller
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet("Items")]
        public IActionResult GetItems()
        {
            return Ok(_itemService.GetItems());
        }

        [HttpGet("Inventory")]
        public IActionResult GetInventory()
        {
            return Ok(_itemService.GetItems(User.GetID()));
        }

        [HttpGet("ActiveItems")]
        public IActionResult GetEquippedItems()
        {
            return Ok(_itemService.GetActiveItems(User.GetID()));
        }

        [HttpPatch("Equip/{itemId}")]
        public IActionResult EquipItem([FromRoute] int itemId)
        {
            _itemService.EquipItem(User.GetID(), itemId);
            return Ok();
        }

        [HttpPatch("Obtain/{itemId}")]
        public IActionResult ObtainItem([FromRoute]int itemId)
        {
            _itemService.ObtainItem(User.GetID(), itemId);
            return Ok();
        }
    }
}
