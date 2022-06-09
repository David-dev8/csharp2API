using QuizRoyaleAPI.Models.Boosters;

namespace QuizRoyaleAPI.Models
{
    public class BoosterFactory
    {
        public Booster getBooster(string boostername)
        {
            switch (boostername)
            {
                case "CategoryIncrease":
                    return new CategoryIncreaseBooster();
                case "FreeSkip":
                    return new FreeSkipBooster();
                case "Hint":
                    return new HintBooster();
                case "ReduceTime":
                    return new ReduceTimeBooster();
                default:
                    return null;
            }
        }
    }
}
