namespace QuizRoyaleAPI.DTOs
{
    /// <summary>
    /// InGamePlayerDTO, Dit is een DTO object voor een InGamePlayer
    /// het verschil tussen een Player en een InGamePlayer is dat een InGamePlayer alleen de data bevat die nuttig is voor een game
    /// </summary>
    public class InGamePlayerDTO
    {
        public string Username { get; set; }

        public string? Title { get; set; }

        public string? ProfilePicture { get; set; }

        public string? Border { get; set; }

        public IEnumerable<ItemDTO> Boosters { get; set; }

        public InGamePlayerDTO(string username, string? title, string? profilePicture, string? border, IEnumerable<ItemDTO> boosters)
        {
            Username = username;
            Title = title;
            ProfilePicture = profilePicture;
            Border = border;
            Boosters = boosters;
        }
    }
}
