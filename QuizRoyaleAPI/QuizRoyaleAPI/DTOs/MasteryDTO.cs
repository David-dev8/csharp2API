namespace QuizRoyaleAPI.DTOs
{
    /// <summary>
    /// MasteryDTO, Dit is een DTO object voor een Mastery
    /// </summary>
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
