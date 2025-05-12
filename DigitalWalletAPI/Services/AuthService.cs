// Services/AuthService.cs

using DigitalWalletAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DigitalWalletAPI.Data;

namespace DigitalWalletAPI.Services
{
    public interface IAuthService
    {
        string GenerateJwtToken(User user);
        bool ValidateUserCredentials(string email, string password);
        bool CheckIfUserExists(string email);
        string HashPassword(string password);
    }

    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public AuthService(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["JWT:Issuer"],
                _configuration["JWT:Audience"],
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateUserCredentials(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
                return false;

            // Verificar se a senha fornecida corresponde à senha hashada armazenada
            var hashedPassword = user.PasswordHash;

            return VerifyPasswordHash(password, hashedPassword);
        }

        public bool CheckIfUserExists(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public string HashPassword(string password)
        {
            // Gerar um salt de 128 bits (16 bytes)
            byte[] salt = new byte[128 / 8];
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            // Criar o hash da senha com o salt
            var hashed = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);

            // Combinar o salt e o hash para que possamos verificar depois
            var saltBase64 = Convert.ToBase64String(salt);
            var hashBase64 = Convert.ToBase64String(hashed);

            return $"{saltBase64}${hashBase64}";
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            var split = storedHash.Split('$');
            if (split.Length != 2)
                return false;

            var saltBase64 = split[0];
            var hashBase64 = split[1];

            var salt = Convert.FromBase64String(saltBase64);
            var storedHashBytes = Convert.FromBase64String(hashBase64);

            var computedHash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);

            return computedHash.SequenceEqual(storedHashBytes);
        }
    }
}
