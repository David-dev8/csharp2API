using QuizRoyaleAPI.DTOs;

namespace QuizRoyaleAPI.Services.Data
{
    /// <summary>
    /// Een interface voor de Item Service
    /// De Item Service is verantwoordelijk voor alles met betrekking tot items
    /// </summary>
    public interface IItemService
    {
        /// <summary>
        /// Haalt alle Items op
        /// </summary>
        /// <returns>Een collectie van ItemDTO's</returns>
        public IEnumerable<ItemDTO> GetItems();

        /// <summary>
        /// Haalt alle items van een speler op
        /// </summary>
        /// <param name="userId">De userID van de speler waarvan je de items op wilt halen</param>
        /// <returns>Een collectie van ItemDTO's</returns>
        public IEnumerable<ItemDTO> GetItems(int userId);

        /// <summary>
        /// Haalt alle actieve items van een speler op
        /// </summary>
        /// <param name="userId">De userID van de speler waarvan je de items op wilt halen</param>
        /// <returns>Een collectie van ItemDTO's</returns>
        public IEnumerable<ItemDTO> GetActiveItems(int userId);

        /// <summary>
        /// Maakt een item actief voor een speler
        /// </summary>
        /// <param name="userId">De userID van de speler waarvan je de item actief wilt maken</param>
        /// <param name="itemId">De itemID van de item die je actief wil maken</param>
        public void EquipItem(int userId, int itemId);

        /// <summary>
        /// Geeft een speler een item
        /// </summary>
        /// <param name="userId">De userID van de speler waaraan je een item wilt geven</param>
        /// <param name="itemId">De itemID van de item die je wilt geven</param>
        public void ObtainItem(int userId, int itemId);
    }
}
