using QuizRoyaleAPI.DTOs;
using QuizRoyaleAPI.Models;

namespace QuizRoyaleAPI.Services.Data
{
    public interface IPlayerService
    {
        public PlayerDetailsDTO GetPlayer(int userId);

        public void GiveRewards(string username, int xp, int coins);

        public void GiveWin(string username);

        public void RegisterResult(string username, Mode mode, int position);

        public int CreatePlayer(string username);

        public void DeletePlayer(int userId);

        public InGamePlayerDTO GetPlayerInGame(string username);
    }
}
