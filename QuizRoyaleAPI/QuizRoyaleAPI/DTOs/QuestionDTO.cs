namespace QuizRoyaleAPI.DTOs
{
    /// <summary>
    /// QuestionDTO, Dit is een DTO object voor een Question
    /// </summary>
    public class QuestionDTO
    {
        public string Content { get; set; }

        public char RightAnswer { get; set; }

        public IEnumerable<AnswerDTO> Possibilities { get; set; }

        public CategoryDTO? Category { get; set; }

        public QuestionDTO(string content, char rightAnswer, IEnumerable<AnswerDTO> possibilities)
        {
            Content = content;
            RightAnswer = rightAnswer;
            Possibilities = possibilities;
        }
    }
}
