namespace DigitalWalletAPI.Data;
using Microsoft.EntityFrameworkCore;
using DigitalWalletAPI.Models;
using System.Security.Cryptography.Xml;


public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Wallet>()
        .HasOne(w => w.User)
        .WithOne(u => u.Wallet)
        .HasForeignKey<Wallet>(w => w.UserId);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.SenderWallet)
            .WithMany(w => w.SentTransactions)
            .HasForeignKey(t => t.SenderWalletId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.ReceiverWallet)
            .WithMany(w => w.ReceivedTransactions)
            .HasForeignKey(t => t.ReceiverWalletId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

