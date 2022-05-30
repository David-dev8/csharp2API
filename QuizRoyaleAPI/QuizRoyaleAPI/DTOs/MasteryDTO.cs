namespace QuizRoyaleAPI.DTOs
{
    public class MasteryDTO
    {
        public CategoryDTO Category { get; set; }
        public double SuccessRate { get; set; }

        public MasteryDTO(CategoryDTO category, double successRate)
        {
            Category = category;
            SuccessRate = successRate;
        }
    }
}
