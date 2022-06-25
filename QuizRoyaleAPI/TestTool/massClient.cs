using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestTool
{
    public class MassClient
    {
        private HubConnection _connection;
        private int _totalPlayers;

        public MassClient(int totalPlayers)
        {
            this._totalPlayers = totalPlayers;
            _connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5264/GameHub")
                .Build();

            _connection.On("StartQuestion", () =>
            {
                AnswerRandomly();
            });

            _connection.On("gameOver", () =>
            {
                _connection.DisposeAsync();
            });
        }

        public async Task Join(string name)
        {
            try
            {
                await _connection.StartAsync();
                await _connection.InvokeAsync("Join", name);
            }
            catch (Exception ex)
            {
            }
        }

        public async Task AnswerRandomly()
        {
            Random random = new Random();
            Thread.Sleep(random.Next(10000 / _totalPlayers));
            int randomInt = random.Next(0, 4);
            char[] charArray = new char[] { 'A', 'B', 'C', 'D' };
            char randomChar = charArray[randomInt];
            await _connection.InvokeAsync("AnswerQuestion", randomChar); ;
        }
    }
}
