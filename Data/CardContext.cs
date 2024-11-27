using Microsoft.EntityFrameworkCore;
using PrimoAPITarjetas.Models;

namespace PrimoAPITarjetas.Data
{
    public class CardContext : DbContext
    {
        public CardContext(DbContextOptions<CardContext> options) : base(options) { }

        public DbSet<Card> Cards { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>()
                .HasKey(c => c.CardId);

            modelBuilder.Entity<Card>()
                .HasIndex(c => c.CardNumber)
                .IsUnique();

            modelBuilder.Entity<Transaction>()
                .HasKey(t => t.TransactionId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Card)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CardId);

            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);
        }
    }
}
