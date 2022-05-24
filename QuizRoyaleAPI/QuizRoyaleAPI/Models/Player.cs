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

        [NotMapped]
        public string? Title
        {
            get
            {
                return GetItemByType(ItemType.TITLE);
            }
        }

        [NotMapped]
        public string? ProfilePicture { 
            get
            {
                return GetItemByType(ItemType.PROFILE_PICTURE);
            }
        }

        [NotMapped]
        public string? Border {
            get
            {
                return GetItemByType(ItemType.BORDER);
            }
        }

        public Player(string username)
        {
            Username = username;
            Results = new List<Result>();
            AcquiredItems = new List<AcquiredItem>();
        }

        private string? GetItemByType(ItemType type)
        {
            return "";
            //return AcquiredItems.Where(i => i.ItemType == type).SingleOrDefault()?.Picture;
        }
    }
}
