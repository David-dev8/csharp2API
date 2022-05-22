using System.ComponentModel.DataAnnotations;

namespace QuizRoyaleAPI.Models
{
    public class Item
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Picture { get; set; }

        public ItemType ItemmType { get; set; }

        public PaymentType PaymentType { get; set; }

        public int Cost { get; set; }

        public IList<Player> PlayersWhoAcquired { get; set; }
    }

    public enum ItemType
    {
        BORDER,
        PROFILE_PICTURE,
        TITLE,
        BOOST
    }

    public enum PaymentType
    {
        XP,
        COINS
    }
}
