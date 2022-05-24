namespace QuizRoyaleAPI.Exceptions
{
    public class InsufficientFundsException : Exception
    {
        public InsufficientFundsException() : base("Not enough funds to obtain the item")
        {
        }
    }
}
