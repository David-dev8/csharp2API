using System.ComponentModel.DataAnnotations;

namespace QuizRoyaleAPI.Models
{
    public class Badge
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Picture { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public BadgeType Type { get; set; }

        public int Gradation { get; set; }

        public virtual ICollection<Player> Players { get; set; }

        public Badge(string name, string picture, string description, BadgeType type, int gradation)
        {
            Name = name;
            Picture = picture;
            Description = description;
            Type = type;
            Gradation = gradation;
            Players = new List<Player>();
        }
    }

    // todo enum string waarde?
    public enum BadgeType
    {
        WINSTREAK,
        TOTAL_WINS,
        ROYALE_PLAYED,
        ITEM_UNLOCKER,
        MASTERY
    }
}
