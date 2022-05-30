using System;
using System.Timers;
using System.Web;
using Microsoft.AspNetCore.SignalR;
using QuizRoyaleAPI.Models;
using QuizRoyaleAPI.Services.Data;

namespace QuizRoyaleAPI.Hubs
{
    public class GameHub : Hub
    {
        private System.Timers.Timer _startDelayTimer;
        private IGameFactory _gameFactory;

        public GameHub(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public async Task join(string username)
        {
            if (State.CurrentGame == null)
            {
                State.CurrentGame = _gameFactory.GetGame(1000);
            }

            if (State.CurrentGame.CanJoin())
            {
                State.CurrentGame.Join(username);

                // Stuurt alleen een melding naar de client die wil joinen
                int playersLeft = State.CurrentGame._minimumPlayers - State.CurrentGame.GetAmountOfPlayers();
                if (playersLeft > 0)
                {
                    string message = "welkom, we wachten nog op " + playersLeft + " spelers";
                    await Clients.Client(Context.ConnectionId).SendAsync("succes", message);
                }

                if (State.CurrentGame.CanStart())
                {
                    string message = "het spel begint binnenkort";
                    await Clients.Client(Context.ConnectionId).SendAsync("updatestatus", message);
                    this.SetStartTimer();
                }
            }
            else 
            {
                // Stuurt alleen een melding naar de client die wil joinen
                await Clients.Client(Context.ConnectionId).SendAsync("failed", "je kan nu niet joinen");
            }
        }

        private void SetStartTimer()
        {
            if (_startDelayTimer != null)
            {
                // Reset de timer als en nieuwe deelnemer binnekomt
                _startDelayTimer.Stop();
                _startDelayTimer.Start();
            }
            else 
            { 
                // Create a timer with a variable interval.
                _startDelayTimer = new System.Timers.Timer(5000);
                // Hook up the Tick event for the timer. 
                _startDelayTimer.Elapsed += this.start;
                _startDelayTimer.AutoReset = false;
                _startDelayTimer.Enabled = true;
            }
        }

        private async void start(Object source, ElapsedEventArgs e)
        {
            State.CurrentGame.Start();
            await Clients.All.SendAsync("start");
        }

        public async Task leave()
        {
            await Clients.All.SendAsync("leave");
        }

        public async Task answerQuestion(int roomID, char AwnserID)
        {
            await Clients.All.SendAsync("answerQuestion");
        }

        public async Task useBoost(int roomID, string type)
        {
            await Clients.All.SendAsync("useBoost");
        }
    }
}

// Socket function template
//
//public async Task FunctionName(Parameter)
//{
//    await Clients.All.SendAsync("FunctionID", ParametersToSend);
//}