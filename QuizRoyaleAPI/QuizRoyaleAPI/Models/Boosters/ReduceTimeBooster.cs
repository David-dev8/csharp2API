using Microsoft.AspNetCore.SignalR;

namespace QuizRoyaleAPI.Models.Boosters
{
    public class ReduceTimeBooster : Booster
    {
        public void use(Game game, string options)
        {
            game._timer.Stop();
            game.SetTimer(2000);
            this.Anounce();
        }

        private async Task Anounce()
        {
            await State.GetHubContext().Clients.All.SendAsync("reduceTime");// Documented
        }
    }
}
