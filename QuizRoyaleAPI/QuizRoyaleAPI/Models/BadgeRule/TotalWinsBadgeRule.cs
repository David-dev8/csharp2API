namespace QuizRoyaleAPI.Models.BadgeRule
{
    /// <summary>
    /// Dit is de badgeRule voor de totalwins Badge
    /// </summary>
    public class TotalWinsBadgeRule : IBadgeRule
    {
        public bool HasEarned(Player player)
        {
            return player.Results.Where(r => r.Position == 1).Count() > 1;
        }
    }
}
