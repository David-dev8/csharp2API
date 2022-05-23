using QuizRoyaleAPI.DataAccess;
using QuizRoyaleAPI.Models;

namespace QuizRoyaleAPI.Services
{
    public class DbPlayerService : IPlayerService
    {
        private readonly QuizRoyaleDbContext _context;

        public DbPlayerService(QuizRoyaleDbContext context)
        {
            _context = context;
        }

        public int CreatePlayer(string username)
        {
            var player = new Player(username);
            _context.Players.Add(player);
            _context.SaveChanges(); // todo duplicate entry
            return player.Id;
        }

        public void DeletePlayer(int userId)
        {
            _context.Players.Remove(GetPlayer(userId));
            _context.SaveChanges();
        }

        public Player GetPlayer(int userId)
        {
            Player? player = _context.Players.Find(userId);
            if(player == null)
            {
                //throw new PlayerNotFoundException();
            }
            return player;
        }
    }
}
