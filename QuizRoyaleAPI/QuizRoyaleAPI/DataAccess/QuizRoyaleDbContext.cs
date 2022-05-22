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

        public DbSet<Category> Categories { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<Rank> Ranks { get; set; }

        public DbSet<Item> Items { get; set; }
    }
}
