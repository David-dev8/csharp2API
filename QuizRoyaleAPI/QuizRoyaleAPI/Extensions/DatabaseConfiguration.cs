using Microsoft.EntityFrameworkCore;
using QuizRoyaleAPI.DataAccess;

namespace QuizRoyaleAPI.Extensions
{
    /// <summary>
    /// DatabaseConfiguration, Deze statische klasse bevat een methode om een database context op te zetten aan de hand van de input.
    /// </summary>
    public static class DatabaseConfiguration
    {
        /// <summary>
        /// Set een context op voor een Database.
        /// </summary>
        /// <param name="services">Alle extensions.</param>
        /// <param name="connectionString">De MySQL conectiestring voor de database waarmee je wilt verbinden.</param>
        public static void AddDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<QuizRoyaleDbContext>(options => options
                .UseLazyLoadingProxies()
                .UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString)));
        }
    }
}
