using QuizRoyaleAPI.DataAccess;
using QuizRoyaleAPI.Exceptions;
using QuizRoyaleAPI.Models;

namespace QuizRoyaleAPI.Services.Data
{
    public class DbItemService : IItemService
    {
        private readonly QuizRoyaleDbContext _context;

        public DbItemService(QuizRoyaleDbContext context)
        {
            _context = context;
        }

        public void ObtainItem(int userId, int itemId)
        {
            Player player = GetPlayer(userId);
            Item item = GetItem(itemId);

            if (CanAfford(item, player))
            {
                player.AcquiredItems.Add(new AcquiredItem { ItemId = item.Id });
            }
            else
            {
                throw new InsufficientFundsException();
            }
        }

        public void EquipItem(int userId, int itemId)
        {
            Player player = GetPlayer(userId);
            Item item = GetItem(itemId);

            if (player.AcquiredItems.Where((i) => i.ItemId == itemId) == null)
            {
                throw new ItemNotFoundException();
            }

            foreach (AcquiredItem playerItem in UnequipItemsOfSameType(player, item.ItemType))
            {
                playerItem.Equipped = playerItem.ItemId == item.Id; // todo transaction?
            }
        }

        public IEnumerable<Item> GetItems()
        {
            return _context.Items;
        }

        public IEnumerable<Item> GetItems(int userId)
        {
            return _context.Items.Where(i => GetPlayer(userId).AcquiredItems.Select(ai => ai.ItemId).Contains(i.Id));
        }

        public IEnumerable<Item> GetActiveItems(int userId)
        {
            throw new NotImplementedException();
        }

        private Player GetPlayer(int userId)
        {
            Player? player = _context.Players.Find(userId);
            if (player == null)
            {
                throw new PlayerNotFoundException();
            }
            return player;
        }

        private bool CanAfford(Item item, Player player)
        {
            int budget = item.PaymentType switch
            {
                PaymentType.XP => player.XP,
                _ => player.Coins,
            };
            return budget >= item.Cost;
        }

        private Item GetItem(int itemId)
        {
            Item? item = _context.Items.Find(itemId);
            if (item == null)
            {
                throw new ItemNotFoundException();
            }
            return item;
        }

        private IEnumerable<AcquiredItem> UnequipItemsOfSameType(Player player, ItemType itemType)
        {
            return player.AcquiredItems
                .Where(ai => _context.Items.Where(i => i.ItemType == itemType).Select(i => i.Id).Contains(ai.ItemId));
        }
    }
}
