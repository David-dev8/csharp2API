using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QuizRoyaleAPI.Enums;
using QuizRoyaleAPI.Models;

namespace QuizRoyaleAPI.DataAccess
{
    /// <summary>
    /// De context voor Entity Framework voor de database die gebruikt wordt.
    /// </summary>
    public class QuizRoyaleDbContext : DbContext
    {
        /// <summary>
        /// Creëert een QuizRoyaleDbContext met de gegeven opties.
        /// </summary>
        /// <param name="options">Opties voor de context.</param>
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
            // Zorg ervoor dat de enums als strings worden opgeslagen in de database.
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

            // Zorg ervoor dat voor de associatietabellen een composite primary key wordt gebruikt
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
