using QuizRoyaleAPI.Models;

namespace QuizRoyaleAPI.DTOs
{
    public class ResultDTO
    {
        public Mode Mode { get; set; }

        public int RightAnswers { get; set; }

        public int? Position { get; set; }

        public ResultDTO(Mode mode, int rightAnswers, int? position)
        {
            Mode = mode;
            RightAnswers = rightAnswers;
            Position = position;
        }
    }
}
