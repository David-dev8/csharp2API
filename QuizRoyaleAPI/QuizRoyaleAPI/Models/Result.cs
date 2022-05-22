using System.ComponentModel.DataAnnotations;

namespace QuizRoyaleAPI.Models
{
    public class Result
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Mode { get; set; }

        public int RightAnswers { get; set; }

        public int Position { get; set; }

        public int PlayerId { get; set; }
    }
}
