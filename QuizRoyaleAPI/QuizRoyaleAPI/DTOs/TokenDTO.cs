namespace QuizRoyaleAPI.DTOs
{
    /// <summary>
    /// TokenDTO, Dit is een DTO object voor een Token.
    /// Tokens worden gebruikt voor authenticatie.
    /// </summary>
    public class TokenDTO
    {
        public string AccessToken { get; set; }

        public TokenDTO(string accessToken)
        {
            AccessToken = accessToken;
        }
    }
}
