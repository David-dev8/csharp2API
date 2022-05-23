using System.ComponentModel.DataAnnotations;

namespace QuizRoyaleAPI.Models
{
    public class Answer
    {
        public int Id { get; set; }

        public char Code { get; set; }
        
        [MaxLength(255)]
        public string Description { get; set; }

        public int QuestionId { get; set; }

        public Answer(char code, string description)
        {
            Code = code;
            Description = description;
        }
    }
}
