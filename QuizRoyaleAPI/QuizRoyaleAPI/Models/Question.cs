using System.ComponentModel.DataAnnotations;

namespace QuizRoyaleAPI.Models
{
    public class Question
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string Content { get; set; }

        public char RightAnswer { get; set; }

        public IList<Answer> Possibilities { get; set; } = new List<Answer>();

        public int CategoryId { get; set; }

        public Question(string content, char rightAnswer, IList<Answer> possibilities)
        {
            Content = content;
            RightAnswer = rightAnswer;
            Possibilities = possibilities;
        }
    }
}
