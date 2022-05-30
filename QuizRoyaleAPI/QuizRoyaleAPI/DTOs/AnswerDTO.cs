namespace QuizRoyaleAPI.DTOs
{
    public class AnswerDTO
    {
        public char Code { get; set; }

        public string Description { get; set; }

        public AnswerDTO(char code, string description)
        {
            Code = code;
            Description = description;
        }
    }
}
