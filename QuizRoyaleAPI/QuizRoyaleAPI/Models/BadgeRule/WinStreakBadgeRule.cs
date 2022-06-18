namespace QuizRoyaleAPI.Models.BadgeRule
{
    /// <summary>
    /// Dit is de badgeRule voor de winstreak badge
    /// </summary>
    public class WinStreakBadgeRule : IBadgeRule
    {
        public bool HasEarned(Player player, int gradation)
        {
            int streak = 0;
            int maxStreak = 0;
            foreach(Result result in player.Results.ToList())
            {
                if(result.Position != 1)
                {
                    streak = 0;
                    continue;
                }

                streak++;
                if(streak > maxStreak)
                {
                    maxStreak = streak;
                }
            }
            return maxStreak >= gradation;
        }
    }
}
