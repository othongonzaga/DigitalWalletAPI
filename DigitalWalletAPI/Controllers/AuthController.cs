using DigitalWalletAPI.Data;
using DigitalWalletAPI.Models;
using DigitalWalletAPI.Services;
using DigitalWalletAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DigitalWalletAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly AppDbContext _context;

        public AuthController(IAuthService authService, AppDbContext context)
        {
            _authService = authService;
            _context = context;
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginRequestDto loginDto)
        {
            if (loginDto == null || string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
            {
                return BadRequest(new { message = "Email e senha são obrigatórios." });
            }

            try
            {
                if (!_authService.ValidateUserCredentials(loginDto.Email, loginDto.Password))
                {
                    return Unauthorized(new { message = "Credenciais inválidas." });
                }

                // Busca o usuário real do banco de dados
                var user = _context.Users.FirstOrDefault(u => u.Email == loginDto.Email);
                if (user == null)
                {
                    return Unauthorized(new { message = "Usuário não encontrado." });
                }

                var token = _authService.GenerateJwtToken(user);

                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno no processo de login.", details = ex.Message });
            }
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] RegisterUserDto userDto)
        {
            if (userDto == null || string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.Password))
            {
                return BadRequest(new { message = "Email e senha são obrigatórios." });
            }

            try
            {
                if (_authService.CheckIfUserExists(userDto.Email))
                {
                    return BadRequest(new { message = "Usuário já existe." });
                }

                var hashedPassword = _authService.HashPassword(userDto.Password);

                var user = new User
                {
                    Name = userDto.Name,
                    Email = userDto.Email,
                    PasswordHash = hashedPassword,
                    Wallet = new Wallet { Balance = 0 }
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(Register), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno ao registrar usuário.", details = ex.Message });
            }
        }


    }
}
