namespace QuizRoyaleAPI.Exceptions
{
    public class UsernameTakenException: Exception
    {
        public UsernameTakenException(): base("The username is already taken")
        {
        }
    }
}
