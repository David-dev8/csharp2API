namespace QuizRoyaleAPI.Models
{
    public class Question
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Content { get; set; }
        public Answer RightAnswer { get; set; }
        public IList<Answer> Possibilities { get; set; }
    }
}
