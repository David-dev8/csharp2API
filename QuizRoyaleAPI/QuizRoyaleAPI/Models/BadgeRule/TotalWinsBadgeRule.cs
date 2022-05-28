namespace QuizRoyaleAPI.Models.BadgeRule
{
    public class TotalWinsBadgeRule : IBadgeRule
    {
        public bool HasEarned(Player player)
        {
            return true;
        }
    }
}
