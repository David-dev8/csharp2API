using Microsoft.EntityFrameworkCore;
using QuizRoyaleAPI.DataAccess;
using QuizRoyaleAPI.Exceptions;
using QuizRoyaleAPI.Models;

namespace QuizRoyaleAPI.Services.Data
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

        public InGamePlayerDTO GetPlayerInGame(string username)
        {
            Player? player = _context.Players.Where(p => p.Username == username).FirstOrDefault();
            if (player == null)
            {
                throw new PlayerNotFoundException();
            }

            IEnumerable<Item> items = _context.Items.Where(i => player.AcquiredItems.Select(ai => ai.ItemId).Contains(i.Id));

            return new InGamePlayerDTO(
                player.Username,
                GetItemByType(items, ItemType.TITLE)?.Picture,
                GetItemByType(items, ItemType.PROFILE_PICTURE)?.Picture,
                GetItemByType(items, ItemType.BORDER)?.Picture,
                new List<BoosterDTO>()
            );
        }

        private Item? GetItemByType(IEnumerable<Item> items, ItemType itemType)
        {
            return items.Where(i => i.ItemType == itemType).SingleOrDefault();
        }
    }
}
