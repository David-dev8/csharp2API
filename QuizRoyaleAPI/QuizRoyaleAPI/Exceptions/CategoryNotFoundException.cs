namespace QuizRoyaleAPI.Exceptions
{
    /// <summary>
    /// PlayerNotFoundException, Een exceptie voor wanneer er geen categorie kan worden gevonden die voldoet aan de gegeven data
    /// </summary>
    public class CategoryNotFoundException : Exception
    {
        public CategoryNotFoundException() : base("The category was not found")
        {
        }
    }
}
