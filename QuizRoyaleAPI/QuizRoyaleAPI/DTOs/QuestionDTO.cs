using System.ComponentModel.DataAnnotations;

namespace QuizRoyaleAPI.Models
{
    public class QuestionDTO
    {
        public string Content { get; set; }

        public IList<Answer> Possibilities { get; set; }
    }
}
