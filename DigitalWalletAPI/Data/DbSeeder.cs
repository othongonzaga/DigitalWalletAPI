using System.Security.Cryptography.Xml;
using DigitalWalletAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DigitalWalletAPI.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Users.Any())
                return;

            var user1 = new User
            {
                Name = "Alice",
                Email = "alice@example.com",
                PasswordHash = "senha123"
            };

            var user2 = new User
            {
                Name = "Bob",
                Email = "bob@example.com",
                PasswordHash = "senha456"
            };

            var user3 = new User
            {
                Name = "Carlos",
                Email = "carlos@example.com",
                PasswordHash = "senha789"
            };

            context.Users.AddRange(user1, user2, user3);
            context.SaveChanges();

            var wallet1 = new Wallet { UserId = user1.Id, Balance = 1000 };
            var wallet2 = new Wallet { UserId = user2.Id, Balance = 750 };
            var wallet3 = new Wallet { UserId = user3.Id, Balance = 500 };

            context.Wallets.AddRange(wallet1, wallet2, wallet3);
            context.SaveChanges();

            var transfer1 = new Transaction
            {
                SenderWalletId = wallet1.Id,
                ReceiverWalletId = wallet2.Id,
                Amount = 200,
                Timestamp = DateTime.UtcNow
            };

            var transfer2 = new Transaction
            {
                SenderWalletId = wallet2.Id,
                ReceiverWalletId = wallet3.Id,
                Amount = 150,
                Timestamp = DateTime.UtcNow
            };

            context.Transactions.AddRange(transfer1, transfer2);
            context.SaveChanges();
        }
    }
}
