using System;
using System.Timers;
using System.Web;
using Microsoft.AspNetCore.SignalR;
using QuizRoyaleAPI.DTOs;
using QuizRoyaleAPI.Models;
using QuizRoyaleAPI.Services.Data;

namespace QuizRoyaleAPI.Hubs
{
    public class GameHub : Hub
    {

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
                    int playersLeft = State.CurrentGame._minimumPlayers - State.CurrentGame.GetAmountOfPlayers();
                    if (playersLeft > 0)
                    {
                        string message = "Welkom, we wachten nog op " + playersLeft + " spelers";
                        IList<InGamePlayerDTO> players = State.CurrentGame.GetPlayerNames();
                        IList<MasteryDTO> categories = State.CurrentGame.getCategories();
                        await Clients.Client(Context.ConnectionId).SendAsync("joinStatus", true, message, players, categories);
                    }

                    InGamePlayerDTO player = State.CurrentGame.GetPlayerObj(username);
                    await Clients.Others.SendAsync("newPlayerJoin", player, "We wachten nog op " + playersLeft + " spelers");

                    if (State.CurrentGame.CanStart())
                    {
                        string message = "Het spel begint binnenkort";
                        await Clients.All.SendAsync("updateStatus", message);
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


        public async Task leave()
        {
            await State.CurrentGame.EliminatePlayer(Context.ConnectionId);
        }

        public async Task answerQuestion(char AwnserID)
        {
            Console.WriteLine(Context.ConnectionId + " wil " + AwnserID + " antwoorden!");
            State.CurrentGame.AnswerQuestion(AwnserID, Context.ConnectionId);
        }

        public async Task useBoost(string type, string options)
        {
            if (options == "")
            {
                options = Context.ConnectionId;
            }
            State.CurrentGame.UseBoost(type, options);
        }
    }
}

// Socket function template
//
//public async Task FunctionName(Parameter)
//{
//    await Clients.All.SendAsync("FunctionID", ParametersToSend);
//}