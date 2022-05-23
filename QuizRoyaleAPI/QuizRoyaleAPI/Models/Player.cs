using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace QuizRoyaleAPI.Models
{
    [Index(nameof(Player.Username), IsUnique = true)]
    public class Player
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Username { get; set; }

        [MaxLength(255)]
        public string? Title { get; set; }

        [MaxLength(255)]
        public string? ProfilePicture { get; set; }

        [MaxLength(255)]
        public string? Border { get; set; }

        public int AmountOfWins { get; set; }

        public int Coins { get; set; }

        public int XP { get; set; }

        public IList<Result> Results { get; set; } = new List<Result>();

        public IList<Item> AcquiredItems { get; set; } = new List<Item>();

        public Player(string username)
        {
            Username = username;
        }
    }
}
