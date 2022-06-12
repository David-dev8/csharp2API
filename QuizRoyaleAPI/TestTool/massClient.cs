using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestTool
{
    public class massClient
    {
        private HubConnection connection;

        public massClient()
        {
            connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5264/GameHub")
                .Build();

            connection.On("StartQuestion", () =>
            {
                awnserRandomly();
            });

            connection.On("gameOver", () =>
            {
                connection.DisposeAsync();
            });
        }

        public async Task join(string name)
        {
            try
            {
                await connection.StartAsync();
                await connection.InvokeAsync("join", name);
            }
            catch (Exception ex)
            {
            }
        }

        public async Task awnserRandomly()
        {
            Random random = new Random();
            Thread.Sleep(random.Next(100));
            int randomInt = random.Next(0, 4);
            char[] charArray = new char[] { 'a', 'b', 'c', 'd' };
            char randomChar = charArray[randomInt];
            await connection.InvokeAsync("answerQuestion", randomChar); ;
        }
    }
}
