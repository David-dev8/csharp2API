using QuizRoyaleAPI.Models;

namespace QuizRoyaleAPI.Services.Data
{
    public interface IItemService
    {
        public IEnumerable<ItemDTO> GetItems();
        
        public IEnumerable<ItemDTO> GetItems(int userId);

        public IEnumerable<ItemDTO> GetActiveItems(int userId);

        public void EquipItem(int userId, int itemId);

        public void ObtainItem(int userId, int itemId);
    }
}
