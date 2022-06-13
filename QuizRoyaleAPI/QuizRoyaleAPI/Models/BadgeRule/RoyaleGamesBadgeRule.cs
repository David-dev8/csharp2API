namespace QuizRoyaleAPI.Models.BadgeRule
{
    /// <summary>
    /// Dit is de badgeRule voor de Royal Games Badge
    /// </summary>
    public class RoyaleGamesBadgeRule : IBadgeRule
    {
        public bool HasEarned(Player player)
        {
            return player.Results.Where(r => r.Mode == Mode.QUIZ_ROYALE).Count() > 1;
        }
    }
}
