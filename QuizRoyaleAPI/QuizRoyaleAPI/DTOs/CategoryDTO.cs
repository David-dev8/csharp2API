namespace QuizRoyaleAPI.DTOs
{
    /// <summary>
    /// CategoryDTO, Dit is een DTO object voor een Category
    /// </summary>
    public class CategoryDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }

        public string Picture { get; set; }

        public CategoryDTO(int id, string name, string color, string picture)
        {
            Id = id;
            Name = name;
            Color = color;
            Picture = picture;
        }
    }
}
