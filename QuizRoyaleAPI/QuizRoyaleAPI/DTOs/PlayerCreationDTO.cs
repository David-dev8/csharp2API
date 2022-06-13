namespace QuizRoyaleAPI.DTOs
{
    /// <summary>
    /// PlayerCreationDTO, Dit is een DTO object voor PlayerCreation
    /// Dit wordt gebruikt zodra een nieuwe speler zich registreerd
    /// </summary>
    public class PlayerCreationDTO
    {
        public string Username { get; set; }

        public PlayerCreationDTO(string username)
        {
            Username = username;
        }
    }
}
