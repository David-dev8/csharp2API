using Microsoft.AspNetCore.SignalR;
using QuizRoyaleAPI.Hubs;

namespace QuizRoyaleAPI.Models
{
    public static class State
    {
        public static Game CurrentGame { get; set; }

        public static IServiceProvider ServiceProvider { get; set; }

        public static IHubContext<GameHub> GetHubContext() {
            return ServiceProvider.GetRequiredService<IHubContext<GameHub>>();
        }
    }
}
