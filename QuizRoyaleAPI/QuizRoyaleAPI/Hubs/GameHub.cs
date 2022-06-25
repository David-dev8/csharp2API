using Microsoft.AspNetCore.SignalR;
using QuizRoyaleAPI.DTOs;
using QuizRoyaleAPI.Models;

namespace QuizRoyaleAPI.Hubs
{
    /// <summary>
    /// De GameHub, Dit is de hub waarin de game plaatsvindt.
    /// </summary>
    public class GameHub : Hub
    {
        private const int DEFAULT_QUESTION_TIME = 10000;

        /// <summary>
        /// Deze methode kan worden aangeroepen door een client om te proberen te joinen.
        /// </summary>
        /// <param name="username">De username van de client die wil joinen.</param>
        /// <returns>Een joinstatus event naar de client.</returns>
        public async Task Join(string username)
        {
            InitializeGame();

            if (State.CurrentGame.CanJoin())
            {
                try
                {
                    State.CurrentGame.Join(username, Context.ConnectionId);

                    // Stuurt alleen een melding naar de client die wil joinen
                    int playersLeft = State.CurrentGame.MinimumPlayers - State.CurrentGame.GetAmountOfPlayers();
                    if (playersLeft >= 0)
                    {
                        await SendJoined(playersLeft);
                    }

                    InGamePlayerDTO player = State.CurrentGame.GetPlayerObj(username);
                    await Clients.Others.SendAsync("newPlayerJoin", player, "We are waiting for " + playersLeft + " spelers");

                    if (State.CurrentGame.CanStart())
                    {
                        await Clients.All.SendAsync("updateStatus", "The game will start soon");
                    }
                }
                catch 
                {
                    await SendNotStarted();
                }
            }
            else 
            {
                await SendNotStarted();
            }
        }

        // Initialiseert de game als deze nog niet bestaat
        private void InitializeGame()
        {
            if (State.CurrentGame == null)
            {
                State.CurrentGame = GameFactory.GetGame(DEFAULT_QUESTION_TIME);
            }
        }

        // Stuurt alleen een melding naar de client die wil joinen
        private async Task SendNotStarted()
        {
            await Clients.Client(Context.ConnectionId).SendAsync("joinStatus", false, "", new string[] { }, new List<CategoryIntensityDTO>());
        }

        // Stuurt een melding naar de client die wil joinen dat hij gejoind is en geef hem een geschikte melding
        private async Task SendJoined(int playersLeft)
        {
            string message = playersLeft == 0 ? "The game will start soon" :
                            "Welcome, we are currently waiting for " + playersLeft + " players";
            IList<InGamePlayerDTO> players = State.CurrentGame.GetPlayerNames();
            IList<CategoryIntensityDTO> categories = State.CurrentGame.GetCategories();
            await Clients.Client(Context.ConnectionId).SendAsync("joinStatus", true, message, players, categories);
        }

        /// <summary>
        /// Deze methode kan worden aangeroepen door de client om een spel handmatig te verlaten.
        /// </summary>
        /// <returns>Een gameOver event naar de player die wil leaven.</returns>
        public async Task Leave()
        {
            await State.CurrentGame.EliminatePlayer(Context.ConnectionId);
        }


        /// <summary>
        /// Deze methode kan worden aangeroepen door de client om een vraag te beantwoorden.
        /// </summary>
        /// <param name="awnserID">De ID van het antwoord dat de client geeft.</param>
        /// <returns>Void</returns>
        public async Task AnswerQuestion(char awnserID)
        {
            Console.WriteLine(Context.ConnectionId + " wil " + awnserID + " antwoorden!");
            State.CurrentGame.AnswerQuestion(awnserID, Context.ConnectionId);
        }

        /// <summary>
        /// Deze methode kan worden aangeroepen door de client om een boost te gebruiken.
        /// </summary>
        /// <param name="type">De type van het boost die de speler wil gebruiken.</param>
        /// <param name="options">De opties voor de boost, dit is alleen toepaselijk bij bepaalde boosts.</param>
        /// <returns>Afhankelijk van de boost die gebruikt wordt.</returns>
        public async Task UseBoost(string type, string options)
        {
            if (options == "")
            {
                options = Context.ConnectionId;
            }
            Console.WriteLine(Context.ConnectionId + " Wil een " + type + " booster gebruiken! met de opties: " + options);
            State.CurrentGame.UseBoost(type, options, Context.ConnectionId);
        }
    }
}