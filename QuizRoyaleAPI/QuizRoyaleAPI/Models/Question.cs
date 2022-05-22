using System.ComponentModel.DataAnnotations;

namespace QuizRoyaleAPI.Models
{
    public class Question
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string Content { get; set; }

        public char RightAnswer { get; set; }

        public IList<Answer> Possibilities { get; set; }

        public int CategoryId { get; set; }
    }
}
