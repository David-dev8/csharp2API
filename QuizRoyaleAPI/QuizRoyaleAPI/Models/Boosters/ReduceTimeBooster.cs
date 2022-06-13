using Microsoft.AspNetCore.SignalR;

namespace QuizRoyaleAPI.Models.Boosters
{
    /// <summary>
    /// De reduce time booster
    /// Deze booster verlaagt de resterende tijd voor een vraag naar 2 seconden
    /// </summary>
    public class ReduceTimeBooster : Booster
    {
        /// <summary>
        /// Gebruik de boost
        /// </summary>
        /// <param name="game">De game waarop deze booster moet worden gebruikt</param>
        /// <param name="options">De conectieID van de speler die de boost gebruikt</param>
        public void use(Game game, string options)
        {
            game._timer.Stop();
            game.SetTimer(2000);
            this.Anounce();
        }

        /// <summary>
        /// Laat iedereen in een game weten dat er nog maar 2 seconden over zijn
        /// </summary>
        /// <returns>void</returns>
        private async Task Anounce()
        {
            await State.GetHubContext().Clients.All.SendAsync("reduceTime");// Documented
        }
    }
}
