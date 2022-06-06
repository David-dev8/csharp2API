using QuizRoyaleAPI.Services.Auth;
using QuizRoyaleAPI.Services.Data;
using QuizRoyaleAPI.Services.Data.Database;
using QuizRoyaleAPI.Models;

namespace QuizRoyaleAPI.Extensions
{
    public static class DataServicesConfiguration
    {
        public static void AddDataServices(this IServiceCollection services)
        {
            services.AddScoped<IQuestionService, DbQuestionService>();
            services.AddScoped<IPlayerService, DbPlayerService>();
            services.AddScoped<IItemService, DbItemService>();
            services.AddScoped<IPlayerDataService, DbPlayerDataService>();
            services.AddScoped<IAuthService, UserJWTAuthService>();
        }
    }
}
