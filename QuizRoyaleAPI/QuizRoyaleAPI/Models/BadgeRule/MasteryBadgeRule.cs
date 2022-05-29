namespace QuizRoyaleAPI.Models.BadgeRule
{
    public class MasteryBadgeRule : IBadgeRule
    {
        public bool HasEarned(Player player)
        {
            return player.Mastery.Average(m => m.QuestionsRight / (double)m.AmountOfQuestions) > 1;
        }
    }
}
