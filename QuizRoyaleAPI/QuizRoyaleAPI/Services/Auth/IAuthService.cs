using QuizRoyaleAPI.DTOs;

namespace QuizRoyaleAPI.Services.Auth
{
    public interface IAuthService
    {
        public TokenDTO GetToken(int id);
    }
}
