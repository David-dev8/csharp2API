using Microsoft.AspNetCore.SignalR;
using QuizRoyaleAPI.Hubs;

namespace QuizRoyaleAPI.Models
{
    /// <summary>
    /// Dit is statiche klasse die alle dingen bevat die belangrijk zijn voor een game zodat ze niet worden gedisposed
    /// </summary>
    public static class State
    {
        public static Game CurrentGame { get; set; }

        public static IServiceProvider ServiceProvider { get; set; }

        public static IHubContext<GameHub> GetHubContext()
        {
            return ServiceProvider.GetRequiredService<IHubContext<GameHub>>();
        }
    }
}
