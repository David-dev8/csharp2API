using Microsoft.AspNetCore.SignalR;
using QuizRoyaleAPI.DTOs;

namespace QuizRoyaleAPI.Models.Boosters
{
    public class CategoryIncreaseBooster : Booster
    {

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

        private async Task Anounce(string id)
        {
            await State.GetHubContext().Clients.All.SendAsync("categoryIncrease", id);// Documented
        }
    }
}
