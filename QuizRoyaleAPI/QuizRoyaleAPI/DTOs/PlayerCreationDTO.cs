namespace QuizRoyaleAPI.DTOs
{
    public class PlayerCreationDTO
    {
        public string Username { get; set; }

        public PlayerCreationDTO(string username)
        {
            Username = username;
        }
    }
}
