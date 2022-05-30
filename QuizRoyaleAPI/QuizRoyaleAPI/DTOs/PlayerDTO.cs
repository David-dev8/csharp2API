namespace QuizRoyaleAPI.DTOs
{
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
