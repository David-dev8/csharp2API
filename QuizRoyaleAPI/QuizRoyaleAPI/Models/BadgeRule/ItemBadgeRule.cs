namespace QuizRoyaleAPI.Models.BadgeRule
{
    /// <summary>
    /// Dit is de BadgeRule coor de Item Badge
    /// </summary>
    public class ItemBadgeRule : IBadgeRule
    {
        public bool HasEarned(Player player, int gradation)
        {
            return player.AcquiredItems.Count() >= gradation;
        }
    }
}
