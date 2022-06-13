namespace QuizRoyaleAPI.Exceptions
{
    /// <summary>
    /// InsufficientFundsException, Een exceptie voor waarneer een speler onvoldoende valuta voor een aankoop
    /// </summary>
    public class InsufficientFundsException : Exception
    {
        public InsufficientFundsException() : base("Not enough funds to obtain the item")
        {
        }
    }
}
