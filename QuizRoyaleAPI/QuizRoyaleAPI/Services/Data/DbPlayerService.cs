using Microsoft.EntityFrameworkCore;
using QuizRoyaleAPI.DataAccess;
using QuizRoyaleAPI.Exceptions;
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
            if (_context.Players.Any(p => p.Username == username))
            {
                throw new UsernameTakenException();
            }
            _context.Players.Add(player);
            _context.SaveChanges();
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
                throw new PlayerNotFoundException();
            }
            return player;
        }
    }
}
