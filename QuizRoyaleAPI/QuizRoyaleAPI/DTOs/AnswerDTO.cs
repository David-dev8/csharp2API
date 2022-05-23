using System.ComponentModel.DataAnnotations;

namespace QuizRoyaleAPI.Models
{
    public class AnswerDTO
    {
        public char Code { get; set; }
        
        public string Description { get; set; }
    }
}
