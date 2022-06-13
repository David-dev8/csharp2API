namespace QuizRoyaleAPI.Exceptions
{
    /// <summary>
    /// UnsupportedRuleException, Een exceptie voor waarneer een bepaalde regel niet kan worden toegevoegd een batch
    /// </summary>
    public class UnsupportedRuleException : Exception
    {
        public UnsupportedRuleException() : base("The specified rule is not supported")
        {
        }
    }
}
