using QuizRoyaleAPI.DTOs;
using QuizRoyaleAPI.Models;

namespace QuizRoyaleAPI.Services.Data
{
    /// <summary>
    /// De interface voor een player service
    /// Een player service regelt alles met betrekking tot spelers
    /// </summary>
    public interface IPlayerService
    {
        /// <summary>
        /// Haalt een playerDetailsDTO op aan de hand van een userID
        /// </summary>
        /// <param name="userId">De userID van de player</param>
        /// <returns>Een PlayerDetailsDTO</returns>
        public PlayerDetailsDTO GetPlayer(int userId);

        /// <summary>
        /// Geeft beloningen aan een speler
        /// </summary>
        /// <param name="username">De username van de speler waaran je beloningen wilt geven</param>
        /// <param name="xp">De hoeveelheid XP dat je wilt geven</param>
        /// <param name="coins">De hoeveelhijd coins die je wilt geven</param>
        public void GiveRewards(string username, int xp, int coins);

        /// <summary>
        /// Increment de Win counter van een speler
        /// </summary>
        /// <param name="username">De username van de speler waaraan je een win wilt geven</param>
        public void GiveWin(string username);

        /// <summary>
        /// Legt een resultaat voor een speler vast
        /// </summary>
        /// <param name="username">De username van de speler waarvan je de resultaten wilt vastleggen</param>
        /// <param name="mode">De gamemode waarvan je resultaten wilt vastleggen</param>
        /// <param name="position">De positie waarin de speler is geindigd</param>
        public void RegisterResult(string username, Mode mode, int position);

        /// <summary>
        /// Removed een item van een spelers inventory
        /// </summary>
        /// <param name="username">De username van de speler waarvan je de item wilt removen</param>
        /// <param name="itemName">De item die je wilt removen</param>
        public void removeItem(string username, string itemName);

        /// <summary>
        /// Maakt een speler aan
        /// </summary>
        /// <param name="username">De username van de nieuwe speler</param>
        /// <returns>Het ID van de speler die zojuist is gecreerd</returns>
        public int CreatePlayer(string username);

        /// <summary>
        /// Verwijderd een speler
        /// </summary>
        /// <param name="userId">De userID van de speler die je wilt verwijderen</param>
        public void DeletePlayer(int userId);

        /// <summary>
        /// Haalt een InGamePlayerDTO op
        /// </summary>
        /// <param name="username">De username van de speler waarvan je een InGamePlayerDTO wilt hebben</param>
        /// <returns>Een InGamePlayerDTO</returns>
        public InGamePlayerDTO GetPlayerInGame(string username);
    }
}
