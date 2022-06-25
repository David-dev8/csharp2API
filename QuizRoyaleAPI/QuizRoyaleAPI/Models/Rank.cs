using System.ComponentModel.DataAnnotations;

namespace QuizRoyaleAPI.Models
{
    /// <summary>
    /// Dit is een Rank object.
    /// </summary>
    public class Rank
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(30)]
        public string Color { get; set; }

        public virtual IList<Division> Divisions { get; set; }

        public Rank(string name, string color)
        {
            Name = name;
            Color = color;
            Divisions = new List<Division>();
        }
    }
}
