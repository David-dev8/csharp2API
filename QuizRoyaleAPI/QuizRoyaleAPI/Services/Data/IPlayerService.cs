using QuizRoyaleAPI.DTOs;

namespace QuizRoyaleAPI.Services.Data
{
    public interface IPlayerService
    {
        public PlayerDetailsDTO GetPlayer(int userId);

        public void GiveRewards(string username, int xp, int coins);

        public void GiveWin(string username);

        public int CreatePlayer(string username);

        public void DeletePlayer(int userId);

        public InGamePlayerDTO GetPlayerInGame(string username);
    }
}
