namespace QuizRoyaleAPI.DTOs
{
    /// <summary>
    /// AnswerDTO, Dit is een DTO object voor een Awnser.
    /// </summary>
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
