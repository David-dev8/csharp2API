using System.ComponentModel.DataAnnotations;

namespace QuizRoyaleAPI.Models
{
    public class Player
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Username { get; set; }

        [MaxLength(255)]
        public string Title { get; set; }

        [MaxLength(255)]
        public string ProfilePicture { get; set; }

        [MaxLength(255)]
        public string Border { get; set; }

        public int AmountOfWins { get; set; }

        public IList<Result> Results { get; set; }

        public IList<Item> AcquiredItems { get; set; }
    }
}
