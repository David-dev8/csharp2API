namespace QuizRoyaleAPI.Models
{
    /// <summary>
    /// Dit is het categoryMastery object.
    /// </summary>
    public class CategoryMastery
    {
        public int CategoryId { get; set; }

        public int PlayerId { get; set; }

        public int AmountOfQuestions { get; set; }

        public int QuestionsRight { get; set; }

        public CategoryMastery(int amountOfQuestions, int questionsRight)
        {
            AmountOfQuestions = amountOfQuestions;
            QuestionsRight = questionsRight;
        }
    }
}
