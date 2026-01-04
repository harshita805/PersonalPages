using Microsoft.IdentityModel.Tokens;
using PersonalPages.Models;
using PersonalPages.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PersonalPages.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthService(IAuthRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        public void Register(RegisterDto dto)
        {
            if (_repo.UserExists(dto.Email))
                throw new Exception("User already exists");

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = Hash(dto.Password),
                Gender = dto.Gender,
                DateOfBirth = dto.DateOfBirth
            };

            _repo.Register(user);
        }

        public string Login(LoginDto dto)
        {
            var hash = _repo.GetPasswordHash(dto.Email);

            if (hash == null || Hash(dto.Password) != hash)
                throw new Exception("Invalid credentials");

            return GenerateJwt(dto.Email);
        }

        private static string Hash(string input)
        {
            using var sha = SHA256.Create();
            return Convert.ToBase64String(
                sha.ComputeHash(Encoding.UTF8.GetBytes(input)));
        }

        private string GenerateJwt(string email)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, email),   // ✅ MAIN CLAIM
                new Claim(ClaimTypes.Email, email)   // (Optional but good)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["JwtKey"])
            );

            var creds = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
