namespace DigitalWalletAPI.Models;

public class Transaction
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public int SenderWalletId { get; set; }
    public Wallet SenderWallet { get; set; }

    public int ReceiverWalletId { get; set; }
    public Wallet ReceiverWallet { get; set; }
}
