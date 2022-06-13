using QuizRoyaleAPI.DataAccess;
using QuizRoyaleAPI.DTOs;
using QuizRoyaleAPI.Exceptions;
using QuizRoyaleAPI.Models;

namespace QuizRoyaleAPI.Services.Data.Database
{
    public class DbPlayerService : IPlayerService
    {
        private readonly QuizRoyaleDbContext _context;

        public DbPlayerService(QuizRoyaleDbContext context)
        {
            _context = context;
        }

        public void GiveRewards(string username, int xp, int coins)
        {
            Player? player = _context.Players.Where(p => p.Username == username).FirstOrDefault();
            player.XP += xp;
            player.Coins += coins;
            _context.SaveChanges();
        }

        public void GiveWin(string username)
        {
            Player? player = _context.Players.Where(p => p.Username == username).FirstOrDefault();
            player.AmountOfWins += 1;
            _context.SaveChanges();
        }

        public void RegisterResult(string username, Mode mode, int position)
        {
            Player? player = _context.Players.Where(p => p.Username == username).FirstOrDefault();
            if(player != null)
            {
                player.Results.Add(new Result(mode, position));
                _context.SaveChanges();
            }
        }

        public int CreatePlayer(string username)
        {
            var player = new Player(username);
            // Controleer of de gegeven gebruikersnaam niet al gekozen is
            if (_context.Players.Any(p => p.Username == username))
            {
                throw new UsernameTakenException();
            }
            _context.Players.Add(player);
            SetDefaultItems(player);
            _context.SaveChanges();
            
            return player.Id;
        }

        public void DeletePlayer(int userId)
        {
            _context.Players.Remove(GetPlayerFromDB(userId));
            _context.SaveChanges();
        }

        public PlayerDetailsDTO GetPlayer(int userId)
        {
            Player player = GetPlayerFromDB(userId);
            return new PlayerDetailsDTO(
                player.Username,
                player.Coins,
                player.XP,
                player.AmountOfWins
            );
        }

        public InGamePlayerDTO GetPlayerInGame(string username)
        {
            Player? player = _context.Players.Where(p => p.Username == username).FirstOrDefault();
            if (player == null)
            {
                throw new PlayerNotFoundException();
            }

            IEnumerable<int> acquiredItemsIds = player.AcquiredItems.Select(ai => ai.ItemId).ToList();
            IEnumerable<Item> items = _context.Items.Where(i => acquiredItemsIds.Contains(i.Id));

            return new InGamePlayerDTO(
                player.Username,
                GetSingleItemByType(items, ItemType.TITLE)?.Picture,
                GetSingleItemByType(items, ItemType.PROFILE_PICTURE)?.Picture,
                GetSingleItemByType(items, ItemType.BORDER)?.Picture,
                GetItemsByType(items, ItemType.BOOST)
            );
        }

        private Player GetPlayerFromDB(int userId)
        {
            Player? player = _context.Players.Find(userId);
            if (player == null)
            {
                throw new PlayerNotFoundException();
            }
            return player;
        }

        private Item? GetSingleItemByType(IEnumerable<Item> items, ItemType itemType)
        {
            return items.Where(i => i.ItemType == itemType).SingleOrDefault();
        }

        private IEnumerable<ItemDTO> GetItemsByType(IEnumerable<Item> items, ItemType itemType)
        {
            return items.Where(i => i.ItemType == itemType).Select(i =>
            {
                return new ItemDTO(
                    i.Id,
                    i.Name,
                    i.Picture,
                    i.ItemType,
                    i.PaymentType,
                    i.Cost, i.Description);
            }).ToList();
        }

        private IEnumerable<Item> GetFreeItems()
        {
            return _context.Items.Where(i => i.Cost == 0).ToList();
        }

        private void SetDefaultItems(Player player)
        {
            IEnumerable<Item> freeItems = GetFreeItems();
            AddDefaultItem(player, freeItems, ItemType.BORDER);
            AddDefaultItem(player, freeItems, ItemType.PROFILE_PICTURE);
            AddDefaultItem(player, freeItems, ItemType.TITLE);
        }

        private void AddDefaultItem(Player player, IEnumerable<Item> items, ItemType itemType)
        {
            Item? item = GetSingleItemByType(items, itemType);
            if(item != null)
            {
                player.AcquiredItems.Add(new AcquiredItem {ItemId = item.Id, Equipped = true });
            }
        }
    }
}
