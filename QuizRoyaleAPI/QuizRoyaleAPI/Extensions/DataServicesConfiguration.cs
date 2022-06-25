using QuizRoyaleAPI.Services.Auth;
using QuizRoyaleAPI.Services.Data;
using QuizRoyaleAPI.Services.Data.Database;

namespace QuizRoyaleAPI.Extensions
{

    /// <summary>
    /// DataServicesConfiguration, Deze statiche klasse bevat een methode om alle services toe te voegen aan de program
    /// </summary>
    public static class DataServicesConfiguration
    {

        /// <summary>
        /// Voegt alle services toe aan de program
        /// </summary>
        /// <param name="services">Alle extensions</param>
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
