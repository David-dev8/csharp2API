using System;
using System.Web;
using Microsoft.AspNetCore.SignalR;
using QuizRoyaleAPI.Models;

namespace QuizRoyaleAPI.Hubs
{
    public class GameHub : Hub
    {
        private Game game { get; set; }
        private Timer startDelayTimer { get; set; }

        public async Task join(string username)
        {
            await Clients.All.SendAsync("join");
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