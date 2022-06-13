namespace QuizRoyaleAPI.Models.BadgeRule
{
    /// <summary>
    /// Dit is de badgeRule voor de Mastery Badge
    /// </summary>
    public class MasteryBadgeRule : IBadgeRule
    {
        public bool HasEarned(Player player, int gradation)
        {
            List<CategoryMastery> mastery = player.Mastery.ToList();
            if(!mastery.Any())
            {
                return false;
            }

            return 100 * mastery.Average(m => m.QuestionsRight / (double)m.AmountOfQuestions) >= gradation;
        }
    }
}
