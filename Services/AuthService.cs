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
        private readonly IUserRepository _userRepo;

        public AuthService(IAuthRepository repo, IConfiguration config, IUserRepository userRepository)
        {
            _repo = repo;
            _config = config;
            _userRepo = userRepository;
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
            var user = _userRepo.GetUserByEmail(dto.Email);

            if (user == null || Hash(dto.Password) != user.PasswordHash)
                throw new Exception("Invalid credentials");

            // ✅ Pass FULL user object
            return GenerateJwtToken(user);
        }

        private static string Hash(string input)
        {
            using var sha = SHA256.Create();
            return Convert.ToBase64String(
                sha.ComputeHash(Encoding.UTF8.GetBytes(input)));
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim("UserId", user.UserId.ToString())   // ✅ NOW POSSIBLE
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
