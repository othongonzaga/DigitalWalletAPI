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
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/user
        [HttpGet]
        public async Task<ActionResult<User>> GetUser()
        {
            try
            {
                // Extraímos o UserId do token do usuário autenticado
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "NameIdentifier")?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                {
                    return BadRequest(new { message = "ID de usuário inválido ou não encontrado no token." });
                }

                // Tentando buscar o usuário no banco
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    return NotFound(new { message = "Usuário não encontrado." });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno ao buscar usuário", details = ex.Message });
            }
        }
    }
}
