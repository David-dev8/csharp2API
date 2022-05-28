namespace QuizRoyaleAPI.Exceptions
{
    public class ItemNotFoundException: Exception
    {
        public ItemNotFoundException() : base("The requested item was not found")
        {
        }
    }
}
