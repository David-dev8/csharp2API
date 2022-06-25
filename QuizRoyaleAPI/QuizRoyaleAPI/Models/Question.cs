using System.ComponentModel.DataAnnotations;

namespace QuizRoyaleAPI.Models
{
    /// <summary>
    /// Dit is een Question object.
    /// </summary>
    public class Question
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string Content { get; set; }

        public char RightAnswer { get; set; }

        public virtual ICollection<Answer> Possibilities { get; set; }

        public int CategoryId { get; set; }

        public Question(string content, char rightAnswer)
        {
            Content = content;
            RightAnswer = rightAnswer;
            Possibilities = new List<Answer>();
        }
    }
}
