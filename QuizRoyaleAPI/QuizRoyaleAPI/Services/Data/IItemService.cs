using QuizRoyaleAPI.Models;

namespace QuizRoyaleAPI.Services
{
    public interface IItemService
    {
        public IEnumerable<Item> GetItems();
        
        public IEnumerable<Item> GetItems(int userId);

        public IEnumerable<Item> EquipItem(int userId, int itemId);

        public IEnumerable<Item> BuyItem(int userId, int itemId);
    }
}
