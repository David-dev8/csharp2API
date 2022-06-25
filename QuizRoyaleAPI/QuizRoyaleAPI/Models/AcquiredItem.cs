namespace QuizRoyaleAPI.Models
{
    /// <summary>
    /// Dit is het acquiredItem object.
    /// </summary>
    public class AcquiredItem
    {
        public int ItemId { get; set; }

        public int PlayerId { get; set; }

        public bool Equipped { get; set; }
    }
}
