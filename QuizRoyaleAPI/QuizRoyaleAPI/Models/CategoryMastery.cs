namespace QuizRoyaleAPI.Models
{
    public class CategoryMastery
    {
        public int CategoryId { get; set; }

        public int PlayerId { get; set; }

        public int AmountOfQuestions { get; set; }

        public int QuestionsRight { get; set; }
    }
}
