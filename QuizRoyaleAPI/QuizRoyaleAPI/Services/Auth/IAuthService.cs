using QuizRoyaleAPI.DTOs;

namespace QuizRoyaleAPI.Services.Auth
{
    /// <summary>
    /// De interface voor de Authorisatie service.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Haalt de token op van een speler.
        /// </summary>
        /// <param name="id">De userID van de speler waarvan je de token wilt hebben.</param>
        /// <returns>Een TokenDTO.</returns>
        public TokenDTO GetToken(int id);
    }
}
