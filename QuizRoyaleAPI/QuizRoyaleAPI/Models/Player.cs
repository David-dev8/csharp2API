using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace QuizRoyaleAPI.Models
{
    /// <summary>
    /// Dit is een Player object.
    /// </summary>
    [Index(nameof(Player.Username), IsUnique = true)]
    public class Player
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Username { get; set; }

        public int AmountOfWins { get; set; }

        public int Coins { get; set; }

        public int XP { get; set; }

        public virtual ICollection<Result> Results { get; set; }

        public virtual ICollection<AcquiredItem> AcquiredItems { get; set; }

        public virtual ICollection<CategoryMastery> Mastery { get; set; }

        public virtual ICollection<Badge> Badges { get; set; }

        public Player(string username)
        {
            Username = username;
            Results = new List<Result>();
            Mastery = new List<CategoryMastery>();
            AcquiredItems = new List<AcquiredItem>();
            Badges = new List<Badge>();
        }
    }
}
