using Microsoft.EntityFrameworkCore;
using QuizRoyaleAPI.DataAccess;

namespace QuizRoyaleAPI.Extensions
{
    public static class DatabaseConfiguration
    {
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
