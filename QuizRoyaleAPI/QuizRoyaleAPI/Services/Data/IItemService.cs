using QuizRoyaleAPI.Models;

namespace QuizRoyaleAPI.Services.Data
{
    public interface IItemService
    {
        public IEnumerable<Item> GetItems();
        
        public IEnumerable<Item> GetItems(int userId);

        public IEnumerable<Item> GetActiveItems(int userId);

        public void EquipItem(int userId, int itemId);

        public void ObtainItem(int userId, int itemId);
    }
}
