using Microsoft.AspNetCore.SignalR;
using QuizRoyaleAPI.DTOs;

namespace QuizRoyaleAPI.Models.Boosters
{
    /// <summary>
    /// De Hint Booster
    /// Deze booster geeft je een hint in game
    /// </summary>
    public class HintBooster : Booster
    {
        /// <summary>
        /// Gebruik de boost
        /// </summary>
        /// <param name="game">De game waarop deze booster moet worden gebruikt</param>
        /// <param name="options">De conectieID van de speler die de boost gebruikt</param>
        public void use(Game game, string options)
        {
            Random random = new Random();
            IList<AnswerDTO> wrongAnswers = game.CurrentQuestion.Possibilities
                .Where(p => p.Code != game.CurrentQuestion.RightAnswer).OrderBy(x => random.Next()).Take(2).ToList();
            Anounce(options, wrongAnswers);
        }

        /// <summary>
        /// Laat een persoon weten dat een paar gegeven antwoorden onjuist zijn
        /// </summary>
        /// <returns>void</returns>
        private async Task Anounce(string connectionId, IList<AnswerDTO> wrongAnswers)
        {
            await State.GetHubContext().Clients.Client(connectionId).SendAsync("reduceAnswers", wrongAnswers); // Documented
        }
    }
}
