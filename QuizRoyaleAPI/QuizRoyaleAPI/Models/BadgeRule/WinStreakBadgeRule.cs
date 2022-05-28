namespace QuizRoyaleAPI.Models.BadgeRule
{
    public class WinStreakBadgeRule : IBadgeRule
    {
        public bool HasEarned(Player player)
        {
            return true;
        }
    }
}
