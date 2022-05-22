using Microsoft.EntityFrameworkCore;
using QuizRoyaleAPI.Models;

namespace QuizRoyaleAPI.DataAccess
{
    public class QuizRoyaleDbContext : DbContext
    {
        public QuizRoyaleDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Question> Questions { get; set; }
    }
}
