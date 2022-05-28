using System.ComponentModel.DataAnnotations;

namespace QuizRoyaleAPI.Models
{
    public class CategoryDTO
    {
        public string Name { get; set; }

        public string Color { get; set; }

        public string Picture { get; set; }

        public CategoryDTO(string name, string color, string picture)
        {
            Name = name;
            Color = color;
            Picture = picture;
        }
    }
}
