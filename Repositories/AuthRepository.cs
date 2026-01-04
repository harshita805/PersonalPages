using Microsoft.Data.SqlClient;
using PersonalPages.Models;

namespace PersonalPages.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IConfiguration _config;

        public AuthRepository(IConfiguration config)
        {
            _config = config;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));
        }

        public bool UserExists(string email)
        {
            using var con = GetConnection();
            var cmd = new SqlCommand(
                "SELECT COUNT(*) FROM Users WHERE Email=@Email", con);
            cmd.Parameters.AddWithValue("@Email", email);
            con.Open();
            return (int)cmd.ExecuteScalar() > 0;
        }

        public void Register(User user)
        {
            using var con = GetConnection();
            var cmd = new SqlCommand(
                @"INSERT INTO Users 
                (FullName, Email, PasswordHash, Gender, DateOfBirth) 
                VALUES 
                (@FullName,@Email,@PasswordHash,@Gender,@DOB)", con);

            cmd.Parameters.AddWithValue("@FullName", user.FullName);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            cmd.Parameters.AddWithValue("@Gender", user.Gender);
            cmd.Parameters.AddWithValue("@DOB", user.DateOfBirth);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public string GetPasswordHash(string email)
        {
            using var con = GetConnection();
            var cmd = new SqlCommand(
                "SELECT PasswordHash FROM Users WHERE Email=@Email", con);
            cmd.Parameters.AddWithValue("@Email", email);
            con.Open();
            return cmd.ExecuteScalar()?.ToString();
        }
    }
}
