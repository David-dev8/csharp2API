using QuizRoyaleAPI.DataAccess;
using QuizRoyaleAPI.DTOs;
using QuizRoyaleAPI.Enums;
using QuizRoyaleAPI.Exceptions;
using QuizRoyaleAPI.Models;

namespace QuizRoyaleAPI.Services.Data.Database
{
    /// <summary>
    /// DbPlayerService, Een implementatie van de PlayerService die comuniceerd met een Database
    /// </summary>
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
            if(_context.Players.Any(p => p.Username == username))
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
            if(player == null)
            {
                throw new PlayerNotFoundException();
            }

            IEnumerable<int> activeItemsIds = player.AcquiredItems.Where(ai => ai.Equipped).Select(ai => ai.ItemId).ToList();
            IEnumerable<Item> items = _context.Items.Where(i => activeItemsIds.Contains(i.Id));

            return new InGamePlayerDTO(
                player.Username,
                GetSingleItemByType(items, ItemType.TITLE)?.Name,
                GetSingleItemByType(items, ItemType.PROFILE_PICTURE)?.Picture,
                GetSingleItemByType(items, ItemType.BORDER)?.Picture,
                GetItemsByType(items, ItemType.BOOST)
            );
        }

        private Player GetPlayerFromDB(string username)
        {
            return _context.Players.Where(p => p.Username == username).Single();
        }

        private Player GetPlayerFromDB(int userId)
        {
            Player? player = _context.Players.Find(userId);
            if(player == null)
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
                player.AcquiredItems.Add(new AcquiredItem { ItemId = item.Id, Equipped = true });
            }
        }

        public void removeItem(string username, string itemName)
        {
            Player player = GetPlayerFromDB(username);
            Item? item = _context.Items.Where(p => p.Name == itemName).FirstOrDefault();
            AcquiredItem? acquiredItem = player.AcquiredItems.Where(i => i.ItemId == item.Id).FirstOrDefault();
            player.AcquiredItems.Remove(acquiredItem);
            _context.SaveChanges();
        }

        public void RegisterAnswer(string username, bool isCorrect, int questionId)
        {
            Player player = GetPlayerFromDB(username);
            Category category = GetCategoryFromDB(questionId);
            CategoryMastery? mastery = player.Mastery.Where(m => m.CategoryId == category.Id).FirstOrDefault();
            if(mastery != null)
            {
                mastery.AmountOfQuestions++;
                if(isCorrect)
                {
                    mastery.QuestionsRight++;
                }
            }
            else
            {
                player.Mastery.Add(new CategoryMastery(1, isCorrect ? 1 : 0)
                {
                    CategoryId = category.Id,
                });
            }
            _context.SaveChanges();
        }

        private Category GetCategoryFromDB(int questionId)
        {
            int? categoryId = _context.Questions.Find(questionId)?.CategoryId;
            if(categoryId != null)
            {
                Category? category = _context.Categories.Find(categoryId);
                if(category != null)
                {
                    return category;
                }
            }
            throw new CategoryNotFoundException();
        }
    }
}
