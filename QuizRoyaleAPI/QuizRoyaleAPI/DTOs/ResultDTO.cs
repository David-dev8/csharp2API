using QuizRoyaleAPI.Enums;

namespace QuizRoyaleAPI.DTOs
{
    /// <summary>
    /// ResultDTO, Dit is een DTO object voor een Result
    /// </summary>
    public class ResultDTO
    {
        public Mode Mode { get; set; }

        public DateTime Time { get; set; }

        public int Position { get; set; }

        public ResultDTO(Mode mode, DateTime time, int position)
        {
            Mode = mode;
            Time = time;
            Position = position;
        }
    }
}
