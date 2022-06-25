namespace QuizRoyaleAPI.DTOs
{
    /// <summary>
    /// QuestionDTO, Dit is een DTO object voor een Question.
    /// </summary>
    public class QuestionDTO
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public char RightAnswer { get; set; }

        public IEnumerable<AnswerDTO> Possibilities { get; set; }

        public CategoryDTO? Category { get; set; }

        public QuestionDTO(int id, string content, char rightAnswer, IEnumerable<AnswerDTO> possibilities)
        {
            Id = id;
            Content = content;
            RightAnswer = rightAnswer;
            Possibilities = possibilities;
        }
    }
}
