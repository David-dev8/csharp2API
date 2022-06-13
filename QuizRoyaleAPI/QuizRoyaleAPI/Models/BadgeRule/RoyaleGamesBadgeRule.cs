namespace QuizRoyaleAPI.Models.BadgeRule
{
    /// <summary>
    /// Dit is de badgeRule voor de Royal Games Badge
    /// </summary>
    public class RoyaleGamesBadgeRule : IBadgeRule
    {
        public bool HasEarned(Player player, int gradation)
        {
            return player.Results.Where(r => r.Mode == Mode.QUIZ_ROYALE).Count() >= gradation;
        }
    }
}
