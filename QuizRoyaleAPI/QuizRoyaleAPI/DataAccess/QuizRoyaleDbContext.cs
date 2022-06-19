using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QuizRoyaleAPI.Enums;
using QuizRoyaleAPI.Models;

namespace QuizRoyaleAPI.DataAccess
{
    /// <summary>
    /// QuizRoyaleDbContext, TODO david
    /// </summary>
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
            modelBuilder.Entity<Item>(i =>
            {
                i.Property(i => i.ItemType).HasConversion(new EnumToStringConverter<ItemType>());
                i.Property(i => i.PaymentType).HasConversion(new EnumToStringConverter<PaymentType>());
            });

            modelBuilder.Entity<Result>(r =>
            {
                r.Property(r => r.Mode).HasConversion(new EnumToStringConverter<Mode>());
            });

            modelBuilder.Entity<Badge>(b =>
            {
                b.Property(b => b.Type).HasConversion(new EnumToStringConverter<BadgeType>());
            });

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
