namespace QuizRoyaleAPI.Models.Boosters
{
    public class FreeSkipBooster : Booster
    {
        public void use(Game game, string options)
        {
            game.AnswerQuestion('*', options);
        }
    }
}
