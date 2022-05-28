using System.ComponentModel.DataAnnotations;

namespace QuizRoyaleAPI.Models
{
    public class QuestionDTO
    {
        public string Content { get; set; }

        public char RightAnswer { get; set; }

        public IEnumerable<Answer> Possibilities { get; set; }

        public QuestionDTO(string content, char rightAnswer, IEnumerable<Answer> possibilities)
        {
            Content = content;
            RightAnswer = rightAnswer;
            Possibilities = possibilities;
        }
    }
}
