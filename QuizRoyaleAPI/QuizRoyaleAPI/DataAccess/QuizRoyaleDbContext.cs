using Microsoft.EntityFrameworkCore;
using QuizRoyaleAPI.Models;

namespace QuizRoyaleAPI.DataAccess
{
    public class QuizRoyaleDbContext : DbContext
    {
        public QuizRoyaleDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Question> Questions { get; set; } = null!;

        public DbSet<Category> Categories { get; set; } = null!;

        public DbSet<Player> Players { get; set; } = null!;

        public DbSet<Rank> Ranks { get; set; } = null!;

        public DbSet<Item> Items { get; set; } = null!;
    }
}
