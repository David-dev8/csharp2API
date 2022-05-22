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

        public void CreatePlayer(string username)
        {
            _context.Players.Add(new Player()
            {
                Username = username
            });
            _context.SaveChanges();
        }

        public void DeletePlayer(string username)
        {
            _context.Players.Remove(GetPlayerByUsername(username));
            _context.SaveChanges();
        }

        public Player GetPlayerByUsername(string username)
        {
            Player? player = _context.Players.Where(p => p.Username == username).FirstOrDefault();
            if(player == null)
            {
                //throw new PlayerNotFoundException();
            }
            return player;
        }
    }
}
