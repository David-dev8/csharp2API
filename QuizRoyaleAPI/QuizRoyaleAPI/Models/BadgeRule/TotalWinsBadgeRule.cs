namespace QuizRoyaleAPI.Models.BadgeRule
{
    public class TotalWinsBadgeRule : IBadgeRule
    {
        public bool HasEarned(Player player)
        {
            return player.Results.Where(r => r.Position == 1).Count() > 1;
        }
    }
}
