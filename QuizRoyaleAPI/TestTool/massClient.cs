using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
