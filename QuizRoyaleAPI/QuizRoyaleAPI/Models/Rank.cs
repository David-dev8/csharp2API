using System.ComponentModel.DataAnnotations;

namespace QuizRoyaleAPI.Models
{
    public class Rank
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        public string Color { get; set; }

        public IList<Division> Divisions { get; set; }
    }
}
