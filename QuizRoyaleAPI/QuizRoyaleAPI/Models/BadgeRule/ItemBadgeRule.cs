namespace QuizRoyaleAPI.Models.BadgeRule
{
    /// <summary>
    /// Dit is de BadgeRule coor de Item Badge
    /// </summary>
    public class ItemBadgeRule : IBadgeRule
    {
        public bool HasEarned(Player player)
        {
            return player.AcquiredItems.Count() > 1;
        }
    }
}
