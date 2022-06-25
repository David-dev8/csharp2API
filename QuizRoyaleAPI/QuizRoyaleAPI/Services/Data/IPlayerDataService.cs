using QuizRoyaleAPI.DTOs;

namespace QuizRoyaleAPI.Services.Data
{
    /// <summary>
    /// De interface voor een PlayerData service.
    /// Een playerData service regelt alles met betrekking tot extra playerData.
    /// </summary>
    public interface IPlayerDataService
    {
        /// <summary>
        /// Haalt de divisie op van een speler.
        /// </summary>
        /// <param name="userId">De userID van de speler waarvan je de divisie wilt hebben.</param>
        /// <returns>Een DivisionDTO.</returns>
        public DivisionDTO GetDivision(int userId);

        /// <summary>
        /// Haalt de resultaten van een speler op.
        /// </summary>
        /// <param name="userId">De userID van de speler waarvan je de resultaten wilt hebben.</param>
        /// <returns>Een collectie met ResultDTO's.</returns>
        public IEnumerable<ResultDTO> GetResults(int userId);

        /// <summary>
        /// Haalt de masteries van een speler op.
        /// </summary>
        /// <param name="userId">De userID van de speler waarvan je de masteries op wilt halen.</param>
        /// <returns>Een collectie van MasteryDTO's.</returns>
        public IEnumerable<CategoryIntensityDTO> GetMastery(int userId);

        /// <summary>
        /// Haalt de badges op van een speler.
        /// </summary>
        /// <param name="userId">De userID van de speler waarvan je de Badges op wilt halen.</param>
        /// <returns>Een collectie van BadgeDTO's.</returns>
        public IEnumerable<BadgeDTO> GetBadges(int userId);
    }
}
