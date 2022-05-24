using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizRoyaleAPI.Models
{
    public class AcquiredItem
    {
        public int ItemId { get; set; }

        public int PlayerId { get; set; }

        public bool Equipped { get; set; }
    }
}
