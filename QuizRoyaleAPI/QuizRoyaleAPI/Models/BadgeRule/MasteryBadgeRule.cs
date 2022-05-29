namespace QuizRoyaleAPI.Models.BadgeRule
{
    public class MasteryBadgeRule : IBadgeRule
    {
        public bool HasEarned(Player player)
        {
            List<CategoryMastery> mastery = player.Mastery.ToList();
            if(!mastery.Any())
            {
                return false;
            }

            return mastery.Average(m => m.QuestionsRight / (double)m.AmountOfQuestions) > 1;
        }
    }
}
