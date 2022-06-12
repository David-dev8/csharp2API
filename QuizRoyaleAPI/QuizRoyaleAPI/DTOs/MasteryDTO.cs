namespace QuizRoyaleAPI.DTOs
{
    public class MasteryDTO
    {
        public CategoryDTO Category { get; set; }
        public double Mastery { get; set; }

        public MasteryDTO(CategoryDTO category, double mastery)
        {
            Category = category;
            Mastery = mastery;
        }
    }
}
