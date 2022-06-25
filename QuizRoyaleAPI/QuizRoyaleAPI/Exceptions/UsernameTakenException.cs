namespace QuizRoyaleAPI.Exceptions
{
    /// <summary>
    /// UsernameTakenException, Een exceptie voor waarneer een gebruiker wil registreeren met een bestaande naam
    /// </summary>
    public class UsernameTakenException : Exception
    {
        public UsernameTakenException() : base("The username is already taken")
        {
        }
    }
}
