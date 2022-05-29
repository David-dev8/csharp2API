namespace QuizRoyaleAPI.DTOs
{
    public class QuestionDTO
    {
        public string Content { get; set; }

        public char RightAnswer { get; set; }

        public IEnumerable<AnswerDTO> Possibilities { get; set; }

        public QuestionDTO(string content, char rightAnswer, IEnumerable<AnswerDTO> possibilities)
        {
            Content = content;
            RightAnswer = rightAnswer;
            Possibilities = possibilities;
        }
    }
}
