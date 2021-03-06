using QuizRoyaleAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace QuizRoyaleAPI.Models
{
    /// <summary>
    /// Dit is een Item object.
    /// </summary>
    public class Item
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Picture { get; set; }

        public ItemType ItemType { get; set; }

        public PaymentType PaymentType { get; set; }

        public int Cost { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }

        public virtual ICollection<AcquiredItem> PlayersWhoAcquired { get; set; }

        public Item(string name, string picture, ItemType itemType, PaymentType paymentType)
        {
            Name = name;
            Picture = picture;
            ItemType = itemType;
            PaymentType = paymentType;
            PlayersWhoAcquired = new List<AcquiredItem>();
        }
    }
}
