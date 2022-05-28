namespace QuizRoyaleAPI.Models.BadgeRule
{
    public class RoyaleGamesBadgeRule : IBadgeRule
    {
        public bool HasEarned(Player player)
        {
            return true;
        }
    }
}
