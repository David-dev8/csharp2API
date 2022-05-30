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

        public DbSet<Division> Divisions { get; set; } = null!;

        public DbSet<Item> Items { get; set; } = null!;

        public DbSet<Badge> Badges { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AcquiredItem>(entity =>
            {
                entity.HasKey(ai => new { ai.ItemId, ai.PlayerId });
            });

            modelBuilder.Entity<CategoryMastery>(entity =>
            {
                entity.HasKey(cm => new { cm.CategoryId, cm.PlayerId });
            });
        }
    }
}
