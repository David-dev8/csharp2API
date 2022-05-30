namespace QuizRoyaleAPI.Models.BadgeRule
{
    public class RoyaleGamesBadgeRule : IBadgeRule
    {
        public bool HasEarned(Player player)
        {
            return player.Results.Where(r => r.Mode == Mode.QUIZ_ROYALE).Count() > 1;
        }
    }
}
