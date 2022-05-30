namespace QuizRoyaleAPI.Exceptions
{
    public class UnsupportedRuleException : Exception
    {
        public UnsupportedRuleException() : base("The specified rule is not supported")
        {

        }
    }
}
