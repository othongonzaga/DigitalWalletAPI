using DigitalWalletAPI.Data;
using DigitalWalletAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DigitalWalletAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly AppDbContext _context;

    public TransactionsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/transactions?userId=1&startDate=2024-01-01&endDate=2024-12-31
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions(
        [FromQuery] int? userId,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        try
        {
            var query = _context.Transactions
                .Include(t => t.SenderWallet)
                .Include(t => t.ReceiverWallet)
                .AsQueryable();

            if (userId.HasValue)
            {
                query = query.Where(t =>
                    t.SenderWallet.UserId == userId.Value ||
                    t.ReceiverWallet.UserId == userId.Value);
            }

            if (startDate.HasValue)
            {
                query = query.Where(t => t.Timestamp >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(t => t.Timestamp <= endDate.Value);
            }

            var transactions = await query.ToListAsync();

            if (!transactions.Any())
            {
                return NotFound(new { message = "Nenhuma transação encontrada para os filtros aplicados." });
            }

            return Ok(transactions);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro interno no servidor", details = ex.Message });
        }
    }

    // POST: api/transactions
    [HttpPost]
    public async Task<ActionResult<Transaction>> CreateTransaction(Transaction transaction)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "Dados inválidos", details = ModelState });
        }

        try
        {
            var sender = await _context.Wallets.FindAsync(transaction.SenderWalletId);
            var receiver = await _context.Wallets.FindAsync(transaction.ReceiverWalletId);

            // Verifica se as carteiras existem
            if (sender == null || receiver == null)
            {
                return BadRequest(new { message = "Carteiras envolvidas não encontradas." });
            }

            // Verifica se o saldo da carteira do remetente é suficiente
            if (sender.Balance < transaction.Amount)
            {
                return BadRequest(new { message = "Saldo insuficiente." });
            }

            // Atualiza os saldos
            sender.Balance -= transaction.Amount;
            receiver.Balance += transaction.Amount;

            // Define o timestamp da transação
            transaction.Timestamp = DateTime.UtcNow;

            // Adiciona a transação ao banco
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transaction);
        }
        catch (DbUpdateException dbEx)
        {
            return StatusCode(500, new { message = "Erro ao salvar a transação no banco de dados", details = dbEx.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro interno ao criar a transação", details = ex.Message });
        }
    }

    // GET: api/transactions/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Transaction>> GetTransaction(int id)
    {
        try
        {
            var transaction = await _context.Transactions
                .Include(t => t.SenderWallet)
                .Include(t => t.ReceiverWallet)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
            {
                return NotFound(new { message = "Transação não encontrada." });
            }

            return Ok(transaction);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao buscar a transação", details = ex.Message });
        }
    }
}
