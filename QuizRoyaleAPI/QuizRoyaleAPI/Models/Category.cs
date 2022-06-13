using System.ComponentModel.DataAnnotations;

namespace QuizRoyaleAPI.Models
{
    /// <summary>
    /// Dit is het categorie object
    /// </summary>
    public class Category
    {
        public int Id { get; set; }
        
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(30)]
        public string Color { get; set; }

        [MaxLength(255)]
        public string Picture { get; set; }

        public virtual ICollection<Question> Questions { get; set; }

        public virtual ICollection<CategoryMastery> Mastery { get; set; }

        public Category(string name, string color, string picture)
        {
            Name = name;
            Color = color;
            Picture = picture;
            Questions = new List<Question>();
            Mastery = new List<CategoryMastery>();
        }
    }
}
