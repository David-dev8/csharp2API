namespace QuizRoyaleAPI.DTOs
{
    /// <summary>
    /// CategoryIntensityDTO, Dit is een DTO object voor een combinatie van een Category met een intensiteitsniveau, wat een percentage tussen 0 en 100 is.
    /// </summary>
    public class CategoryIntensityDTO
    {
        public CategoryDTO Category { get; set; }
        public double Intensity { get; set; }

        public CategoryIntensityDTO(CategoryDTO category, double intensity)
        {
            Category = category;
            Intensity = intensity;
        }
    }
}
