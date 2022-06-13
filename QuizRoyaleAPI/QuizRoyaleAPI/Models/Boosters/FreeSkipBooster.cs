namespace QuizRoyaleAPI.Models.Boosters
{
    /// <summary>
    /// De Free skip booster
    /// Deze Booster geeft je de optie om niet te antwoorden op een vraag zonder te verliezen
    /// </summary>
    public class FreeSkipBooster : Booster
    {
        /// <summary>
        /// Gebruik de boost
        /// </summary>
        /// <param name="game">De game waarop deze booster moet worden gebruikt</param>
        /// <param name="options">De conectieID van de speler die de boost gebruikt</param>
        public void use(Game game, string options)
        {
            // Antwoorden met een * worden altijd goedgerekend in de game
            game.AnswerQuestion('*', options);
        }
    }
}
