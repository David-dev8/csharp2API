namespace QuizRoyaleAPI.Exceptions
{
    public class PlayerNotFoundException: Exception
    {
        public PlayerNotFoundException(): base("The player was not found")
        {
        }
    }
}
