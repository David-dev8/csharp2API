﻿using System;
using System.Timers;
using System.Web;
using Microsoft.AspNetCore.SignalR;
using QuizRoyaleAPI.Models;
using QuizRoyaleAPI.Services.Data;

namespace QuizRoyaleAPI.Hubs
{
    public class GameHub : Hub
    {

        public async Task join(string username)
        {
            if (State.CurrentGame == null)
            {
                State.CurrentGame = GameFactory.GetGame(10000);
            }

            if (State.CurrentGame.CanJoin())
            {
                State.CurrentGame.Join(username, Context.ConnectionId);

                // Stuurt alleen een melding naar de client die wil joinen
                int playersLeft = State.CurrentGame._minimumPlayers - State.CurrentGame.GetAmountOfPlayers();
                if (playersLeft > 0)
                {
                    string message = "welkom, we wachten nog op " + playersLeft + " spelers";
                    string[] players = State.CurrentGame.GetPlayerNames(); 
                    await Clients.Client(Context.ConnectionId).SendAsync("joinStatus", true, message, players);
                    await Clients.All.SendAsync("newPlayerJoin", username);
                }

                if (State.CurrentGame.CanStart())
                {
                    string message = "het spel begint binnenkort";
                    await Clients.All.SendAsync("updateStatus", message);
                }
            }
            else 
            {
                // Stuurt alleen een melding naar de client die wil joinen
                await Clients.Client(Context.ConnectionId).SendAsync("joinStatus", false, "", new string[] {});
            }
        }


        public async Task leave()
        {
            await State.CurrentGame.EliminatePlayer(Context.ConnectionId);
        }

        public async Task answerQuestion(char AwnserID)
        {
            State.CurrentGame.AnswerQuestion(AwnserID, Context.ConnectionId);
            await Clients.Client(Context.ConnectionId).SendAsync("answerQuestion", AwnserID);
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