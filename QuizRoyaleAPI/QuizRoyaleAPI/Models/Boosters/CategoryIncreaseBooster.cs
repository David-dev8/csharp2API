using Microsoft.AspNetCore.SignalR;
using QuizRoyaleAPI.DTOs;

namespace QuizRoyaleAPI.Models.Boosters
{
    /// <summary>
    /// De categoryIncreaseBooster.
    /// Deze booster verhoogt de kans dat een category gekozen word met 10%.
    /// </summary>
    public class CategoryIncreaseBooster : Booster
    {
        /// <summary>
        /// Gebruik de boost.
        /// </summary>
        /// <param name="game">De game waarin het moet worden gebruikt.</param>
        /// <param name="id">De ID van de categorie die moet worden opgehoogd.</param>
        public void Use(Game game, string id)
        {
            foreach (KeyValuePair<CategoryDTO, float> category in game.Categories)
            {
                if (category.Key.Name == id)
                {
                    game.Categories[category.Key] += 10;
                }
                else 
                {
                    game.Categories[category.Key] -= (float)(10.0 / (game.Categories.Count - 1));
                }

                if (game.Categories[category.Key] < 1)
                {
                    game.Categories[category.Key] = 1;
                }
                if (game.Categories[category.Key] > (101 - game.Categories.Count))
                {
                    game.Categories[category.Key] = (101 - game.Categories.Count);
                }
            }
            this.Anounce(id);
        }

        /// <summary>
        /// Laat alle spelers in het porje weten de de kans van een categorie nu groter is.
        /// </summary>
        /// <param name="id">De id van de categorie die geïncreased wordt</param>
        /// <returns>De id van de geïncreasede categorie.</returns>
        private async Task Anounce(string id)
        {
            await State.GetHubContext().Clients.All.SendAsync("categoryIncrease", id); // Documented
        }
    }
}
