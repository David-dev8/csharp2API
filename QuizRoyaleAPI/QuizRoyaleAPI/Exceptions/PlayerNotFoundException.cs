namespace QuizRoyaleAPI.Exceptions
{
    /// <summary>
    /// PlayerNotFoundException, Een exceptie voor waarneer er geen speler kan worden gevonden die voldoet aan de gegeven data
    /// </summary>
    public class PlayerNotFoundException: Exception
    {
        public PlayerNotFoundException(): base("The player was not found")
        {
        }
    }
}
