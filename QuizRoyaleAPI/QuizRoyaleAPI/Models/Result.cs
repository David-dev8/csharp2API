using QuizRoyaleAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace QuizRoyaleAPI.Models
{
    /// <summary>
    /// Dit een een Result object.
    /// </summary>
    public class Result
    {
        public int Id { get; set; }

        public DateTime Time { get; set; }

        [MaxLength(100)]
        public Mode Mode { get; set; }

        public int Position { get; set; }

        public int PlayerId { get; set; }

        public Result(Mode mode, int position)
        {
            Mode = mode;
            Position = position;
            Time = DateTime.Now;
        }
    }
}
