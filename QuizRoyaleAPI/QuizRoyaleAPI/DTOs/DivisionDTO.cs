using System.ComponentModel.DataAnnotations;

namespace QuizRoyaleAPI.Models
{
    public class DivisionDTO
    {
        public string Rank { get; set; }

        public int Number { get; set; }

        public string Picture { get; set; }

        public double UpperBound { get; set; }
    }
}
