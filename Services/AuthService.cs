using Microsoft.IdentityModel.Tokens;
using PersonalPages.Models;
using PersonalPages.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace PersonalPages.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repo;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository repo, IConfiguration config)
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

            return GenerateJwt();
        }

        private static string Hash(string input)
        {
            using var sha = SHA256.Create();
            return Convert.ToBase64String(
                sha.ComputeHash(Encoding.UTF8.GetBytes(input)));
        }

        private string GenerateJwt()
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["JwtKey"]));

            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddHours(2),
                signingCredentials:
                    new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
