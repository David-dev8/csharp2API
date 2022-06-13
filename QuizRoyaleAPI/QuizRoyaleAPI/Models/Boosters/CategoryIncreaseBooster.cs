using Microsoft.AspNetCore.SignalR;
using QuizRoyaleAPI.DTOs;

namespace QuizRoyaleAPI.Models.Boosters
{
    /// <summary>
    /// De categoryIncreaseBooster
    /// Deze booster verhoogt de kans dat een category gekozen word met 10%
    /// </summary>
    public class CategoryIncreaseBooster : Booster
    {
        /// <summary>
        /// Gebruik de boost
        /// </summary>
        /// <param name="game">De game waarin het moet worden gebrukt</param>
        /// <param name="id">De ID van de categorie die moet worden opgehoogd</param>
        public void use(Game game, string id)
        {
            foreach (KeyValuePair<CategoryDTO, float> category in game._categories)
            {
                if (category.Key.Id.ToString() == id)
                {
                    game._categories[category.Key] += 10;
                }
                else 
                {
                    game._categories[category.Key] -= (float)(10.0 / game._categories.Count);
                }

                if (game._categories[category.Key] < 1)
                {
                    game._categories[category.Key] = 1;
                }
                if (game._categories[category.Key] > (101 - game._categories.Count))
                {
                    game._categories[category.Key] = (101 - game._categories.Count);
                }
            }
            this.Anounce(id);
        }

        /// <summary>
        /// Laat alle spelers in het porje weten de de kans van een categorie nu groter is
        /// </summary>
        /// <param name="id">De id van de categorie die ge-increased word</param>
        /// <returns>De id van de ge-increasede categorie</returns>
        private async Task Anounce(string id)
        {
            await State.GetHubContext().Clients.All.SendAsync("categoryIncrease", id);// Documented
        }
    }
}
