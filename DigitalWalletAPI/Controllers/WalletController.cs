using DigitalWalletAPI.Data;
using DigitalWalletAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DigitalWalletAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WalletController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/wallet
        [HttpGet]
        public async Task<ActionResult<Wallet>> GetWallet()
        {
            try
            {
                // Extraímos o UserId do token do usuário autenticado
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "NameIdentifier")?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                {
                    return BadRequest(new { message = "ID de usuário inválido ou não encontrado no token." });
                }

                // Tentando buscar a carteira do usuário
                var wallet = await _context.Wallets
                    .FirstOrDefaultAsync(w => w.UserId == userId);

                if (wallet == null)
                {
                    return NotFound(new { message = "Carteira não encontrada." });
                }

                return Ok(wallet);
            }
            catch (Exception ex)
            {
                // Captura e retorna erro genérico para falhas inesperadas
                return StatusCode(500, new { message = "Erro interno ao buscar carteira", details = ex.Message });
            }
        }

        // POST: api/wallet
        [HttpPost]
        public async Task<ActionResult<Wallet>> CreateWallet()
        {
            try
            {
                // Extraímos o UserId do token do usuário autenticado
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "NameIdentifier")?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                {
                    return BadRequest(new { message = "ID de usuário inválido ou não encontrado no token." });
                }

                // Verificando se o usuário já possui uma carteira
                var existingWallet = await _context.Wallets
                    .FirstOrDefaultAsync(w => w.UserId == userId);

                if (existingWallet != null)
                {
                    return BadRequest(new { message = "Usuário já possui uma carteira." });
                }

                // Criando uma nova carteira
                var wallet = new Wallet
                {
                    UserId = userId,
                    Balance = 0 
                };

                _context.Wallets.Add(wallet);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetWallet), new { id = wallet.Id }, wallet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno ao criar carteira", details = ex.Message });
            }
        }
    }
}
