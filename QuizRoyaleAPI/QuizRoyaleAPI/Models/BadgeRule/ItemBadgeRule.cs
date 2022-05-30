namespace QuizRoyaleAPI.Models.BadgeRule
{
    public class ItemBadgeRule : IBadgeRule
    {
        public bool HasEarned(Player player)
        {
            return player.AcquiredItems.Count() > 1;
        }
    }
}
