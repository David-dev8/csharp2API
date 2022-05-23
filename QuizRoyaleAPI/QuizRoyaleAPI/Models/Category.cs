using System.ComponentModel.DataAnnotations;

namespace QuizRoyaleAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(30)]
        public string Color { get; set; }

        [MaxLength(255)]
        public string Picture { get; set; }

        public IList<Question> Questions { get; set; } = new List<Question>();

        public Category(string name, string color, string picture)
        {
            Name = name;
            Color = color;
            Picture = picture;
        }
    }
}
