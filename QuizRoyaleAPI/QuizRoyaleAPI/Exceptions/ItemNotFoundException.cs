namespace QuizRoyaleAPI.Exceptions
{
    /// <summary>
    /// ItemNotFoundException, Een exceprie voor waarneer een bepaalde item niet kan worden gevonden
    /// </summary>
    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException() : base("The requested item was not found")
        {
        }
    }
}
