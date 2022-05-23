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

        public ItemType ItemType { get; set; }

        public PaymentType PaymentType { get; set; }

        public int Cost { get; set; }

        public IList<Player> PlayersWhoAcquired { get; set; } = new List<Player>();

        public Item(string name, string picture, ItemType itemType, PaymentType paymentType)
        {
            Name = name;
            Picture = picture;
            ItemType = itemType;
            PaymentType = paymentType;
        }
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
