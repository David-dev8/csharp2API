using QuizRoyaleAPI.Models;

namespace QuizRoyaleAPI.Services
{
    public interface IPlayerService
    {
        public Player GetPlayerByUsername(string username);

        public void CreatePlayer(string username);

        public void DeletePlayer(string username);
    }
}
