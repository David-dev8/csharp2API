using QuizRoyaleAPI.Models;

namespace QuizRoyaleAPI.Services
{
    public interface IPlayerService
    {
        public Player GetPlayer(int userId);

        public int CreatePlayer(string username);

        public void DeletePlayer(int userId);
    }
}
