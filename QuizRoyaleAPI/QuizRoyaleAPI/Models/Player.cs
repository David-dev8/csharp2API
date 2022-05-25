using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizRoyaleAPI.Models
{
    [Index(nameof(Player.Username), IsUnique = true)]
    public class Player
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Username { get; set; }

        public int AmountOfWins { get; set; }

        public int Coins { get; set; }

        public int XP { get; set; }

        public IList<Result> Results { get; set; }

        public IList<AcquiredItem> AcquiredItems { get; set; }

        public Player(string username)
        {
            Username = username;
            Results = new List<Result>();
            AcquiredItems = new List<AcquiredItem>();
        }
    }
}
