using System.ComponentModel.DataAnnotations;

namespace QuizRoyaleAPI.Models
{
    public class QuestionDTO
    {
        public string Content { get; set; }

        public IEnumerable<Answer> Possibilities { get; set; }

        public QuestionDTO(string content, IEnumerable<Answer> possibilities)
        {
            Content = content;
            Possibilities = possibilities;
        }
    }
}
