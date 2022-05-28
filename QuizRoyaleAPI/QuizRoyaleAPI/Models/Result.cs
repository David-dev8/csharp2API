using System.ComponentModel.DataAnnotations;

namespace QuizRoyaleAPI.Models
{
    public class Result
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public Mode Mode { get; set; } // todo mode als string

        public int RightAnswers { get; set; }

        public int? Position { get; set; }

        public int PlayerId { get; set; }

        public Result(Mode mode, int rightAnswers)
        {
            Mode = mode;
            RightAnswers = rightAnswers;
        }
    }

    public enum Mode
    {
        QUIZ_ROYALE
    }
}
