using QuizRoyaleAPI.DataAccess;
using QuizRoyaleAPI.Exceptions;
using QuizRoyaleAPI.Models;

namespace QuizRoyaleAPI.Services.Data
{
    public class DbItemService : IItemService
    {
        private readonly QuizRoyaleDbContext _context;

        private static Dictionary<ItemType, int> s_equipLimits = new()
        {
            [ItemType.TITLE] = 1,
            [ItemType.BORDER] = 1,
            [ItemType.PROFILE_PICTURE] = 1,
            [ItemType.BOOST] = 5
        };

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
            _context.SaveChanges();
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
                if(playerItem.ItemId == item.Id)
                {
                    playerItem.Equipped = true;
                }
            }
            _context.SaveChanges();
        }

        public IEnumerable<ItemDTO> GetItems()
        {
            return _context.Items.Select(ConvertToItemDTO);
        }

        public IEnumerable<ItemDTO> GetItems(int userId)
        {
            IEnumerable<int> AcquiredItemsIDs = GetPlayer(userId).AcquiredItems.Select(ai => ai.ItemId);
            return _context.Items.Where(i => AcquiredItemsIDs.Contains(i.Id)).Select(ConvertToItemDTO);
        }

        public IEnumerable<ItemDTO> GetActiveItems(int userId)
        {
            IEnumerable<int> ActiveItems = GetPlayer(userId).AcquiredItems.Where(ai => ai.Equipped).Select(ai => ai.ItemId);
            return _context.Items.Where(i => ActiveItems.Contains(i.Id)).Select(ConvertToItemDTO);
        }

        private ItemDTO ConvertToItemDTO(Item i)
        {
            return new ItemDTO(
                i.Id,
                i.Name,
                i.Picture,
                i.ItemType,
                i.PaymentType,
                i.Cost, i.Description);
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

        private Item GetItem(int itemId)
        {
            Item? item = _context.Items.Find(itemId);
            if (item == null)
            {
                throw new ItemNotFoundException();
            }
            return item;
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

        private IEnumerable<AcquiredItem> UnequipItemsOfSameType(Player player, ItemType itemType)
        {
            IEnumerable<AcquiredItem> items = player.AcquiredItems
                .Where(ai => _context.Items.Where(i => i.ItemType == itemType).Select(i => i.Id).Contains(ai.ItemId));

            int amountOfTooMuchItems = s_equipLimits[itemType] - items.Count();
            // Voeg 1 toe omdat we een nieuw open slot voor een item willen hebben
            amountOfTooMuchItems++;

            // Unequip alle items die teveel zijn
            foreach(AcquiredItem item in items.Take(amountOfTooMuchItems))
            {
                item.Equipped = false;
            }

            return items;
        }
    }
}
