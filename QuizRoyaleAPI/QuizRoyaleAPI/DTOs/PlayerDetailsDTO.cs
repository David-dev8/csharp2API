namespace QuizRoyaleAPI.DTOs
{
    /// <summary>
    /// PlayerDetailsDTO, Dit is een DTO object voor PlayerDetails.
    /// </summary>
    public class PlayerDetailsDTO
    {
        public string Username { get; set; }

        public int Coins { get; set; }

        public int XP { get; set; }

        public int AmountOfWins { get; set; }

        public PlayerDetailsDTO(string username, int coins, int xp, int amountOfWins)
        {
            Username = username;
            Coins = coins;
            XP = xp;
            AmountOfWins = amountOfWins;
        }
    }
}
