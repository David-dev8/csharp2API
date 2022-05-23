using System.ComponentModel.DataAnnotations;

namespace QuizRoyaleAPI.Models
{
    public class ResultDTO
    { 
        public Mode Mode { get; set; }

        public int RightAnswers { get; set; }

        public int? Position { get; set; }
    }
}
