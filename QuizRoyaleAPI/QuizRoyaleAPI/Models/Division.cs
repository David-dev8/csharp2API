using System.ComponentModel.DataAnnotations;

namespace QuizRoyaleAPI.Models
{
    /// <summary>
    /// Dit is het Divisie object.
    /// </summary>
    public class Division
    {
        public int Id { get; set; }

        public int Number { get; set; }

        [MaxLength(255)]
        public string Picture { get; set; }

        public double UpperBound { get; set; }

        public int RankId { get; set; }

        public Division(int number, string picture, double upperBound)
        {
            Number = number;
            Picture = picture;
            UpperBound = upperBound;
        }
    }
}
