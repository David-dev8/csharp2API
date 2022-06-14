using QuizRoyaleAPI.Models.Boosters;

namespace QuizRoyaleAPI.Models
{
    /// <summary>
    /// Dit is de booster factory
    /// </summary>
    public class BoosterFactory
    {
        /// <summary>
        /// Maakt een booster aan aan de hand van de booster naam
        /// </summary>
        /// <param name="boostername">De naam van het type booster dat je wilt maken</param>
        /// <returns>Een Booster</returns>
        public Booster getBooster(string boostername)
        {
            switch (boostername)
            {
                case "Category increase":
                    return new CategoryIncreaseBooster();
                case "Skip":
                    return new FreeSkipBooster();
                case "Fifty fifty":
                    return new HintBooster();
                case "Hurry!":
                    return new ReduceTimeBooster();
                default:
                    return null;
            }
        }
    }
}
