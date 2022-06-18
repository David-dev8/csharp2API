using System;
using System.Timers;
using System.Web;
using Microsoft.AspNetCore.SignalR;
using QuizRoyaleAPI.DTOs;
using QuizRoyaleAPI.Models;
using QuizRoyaleAPI.Services.Data;

namespace QuizRoyaleAPI.Hubs
{
    /// <summary>
    /// De GameHub, Dit is de hub waarin de game plaatsvindt
    /// </summary>
    public class GameHub : Hub
    {
        /// <summary>
        /// Deze methode kan worden aangeroepen door een client om te proberen te joinen
        /// </summary>
        /// <param name="username">De username van de client die wil joinen</param>
        /// <returns>Een joinstatus event naar de client</returns>
        public async Task join(string username)
        {
            Console.WriteLine(username + " wil Joinen");
            if (State.CurrentGame == null)
            {
                State.CurrentGame = GameFactory.GetGame(10000);
            }

            if (State.CurrentGame.CanJoin())
            {
                try
                {
                    State.CurrentGame.Join(username, Context.ConnectionId);

                    // Stuurt alleen een melding naar de client die wil joinen
                    int playersLeft = State.CurrentGame.MinimumPlayers - State.CurrentGame.GetAmountOfPlayers();
                    if (playersLeft >= 0)
                    {
                        string message = playersLeft == 0 ? "The game will start soon" : 
                            "Welcome, we are currently waiting for " + playersLeft + " players";
                        IList<InGamePlayerDTO> players = State.CurrentGame.GetPlayerNames();
                        IList<MasteryDTO> categories = State.CurrentGame.getCategories();
                        await Clients.Client(Context.ConnectionId).SendAsync("joinStatus", true, message, players, categories);
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
                    // Stuurt alleen een melding naar de client die wil joinen
                    await Clients.Client(Context.ConnectionId).SendAsync("joinStatus", false, "", new string[] { }, new List<MasteryDTO>());
                }
            }
            else 
            {
                // Stuurt alleen een melding naar de client die wil joinen
                await Clients.Client(Context.ConnectionId).SendAsync("joinStatus", false, "", new string[] { }, new List<MasteryDTO>()) ;
            }
        }

        /// <summary>
        /// Deze methode kan worden aangeroepen door de client om een spel handmatig te verlaten
        /// </summary>
        /// <returns>Een gameOver event naar de player die wil leaven</returns>
        public async Task leave()
        {
            await State.CurrentGame.EliminatePlayer(Context.ConnectionId);
        }


        /// <summary>
        /// Deze methode kan worden aangeroepen door de client om een vraag te beantwoorden
        /// </summary>
        /// <param name="AwnserID">De ID van het antwoord dat de client geeft</param>
        /// <returns>Void</returns>
        public async Task answerQuestion(char AwnserID)
        {
            Console.WriteLine(Context.ConnectionId + " wil " + AwnserID + " antwoorden!");
            State.CurrentGame.AnswerQuestion(AwnserID, Context.ConnectionId);
        }

        /// <summary>
        /// Deze methode kan worden aangeroepen door de client om een boost te gebruiken
        /// </summary>
        /// <param name="type">De type van het boost die de speler wil gebruiken</param>
        /// <param name="options">De opties voor de boost, dit is alleen toepaselijk bij bepaalde boosts</param>
        /// <returns>Afhankelijk van de boost die gebruikt wordt</returns>
        public async Task useBoost(string type, string options)
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