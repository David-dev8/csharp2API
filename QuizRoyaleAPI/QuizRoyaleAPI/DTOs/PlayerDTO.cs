namespace QuizRoyaleAPI.DTOs
{
    /// <summary>
    /// PlayerDTO, Dit is een DTO object voor een Player.
    /// deze player bevat niet genoeg data voor het spelen van een game, gebruik daarvoor InGamePlayerDTO.
    /// </summary>
    public class PlayerDTO
    {
        public string Username { get; set; }

        public string Border { get; set; }

        public string ProfilePicture { get; set; }

        public PlayerDTO(string username, string border, string profilePicture)
        {
            Username = username;
            Border = border;
            ProfilePicture = profilePicture;
        }
    }
}
