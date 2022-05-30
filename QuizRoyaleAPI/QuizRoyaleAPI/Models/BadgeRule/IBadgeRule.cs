namespace QuizRoyaleAPI.Models.BadgeRule
{
    public interface IBadgeRule
    {
        public bool HasEarned(Player player);
    }
}
