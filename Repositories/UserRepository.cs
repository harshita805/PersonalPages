using Microsoft.Data.SqlClient;
using PersonalPages.Models;

namespace PersonalPages.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _config;

        public UserRepository(IConfiguration config)
        {
            _config = config;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));
        }

        public User GetUserByEmail(string email)
        {
            using var con = GetConnection();
            var cmd = new SqlCommand(
                "SELECT FullName, Email, Gender, DateOfBirth FROM Users WHERE Email=@Email", con);
            cmd.Parameters.AddWithValue("@Email", email);

            con.Open();
            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return new User
            {
                FullName = reader["FullName"].ToString(),
                Email = reader["Email"].ToString(),
                Gender = reader["Gender"].ToString(),
                DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"])
            };
        }

        public void UpdateUser(string email, UpdateUserDto dto)
        {
            using var con = GetConnection();
            var cmd = new SqlCommand(
                @"UPDATE Users 
          SET FullName=@FullName, Gender=@Gender, DateOfBirth=@DOB
          WHERE Email=@Email", con);

            cmd.Parameters.AddWithValue("@FullName", dto.FullName);
            cmd.Parameters.AddWithValue("@Gender", dto.Gender);
            cmd.Parameters.AddWithValue("@DOB", dto.DateOfBirth);
            cmd.Parameters.AddWithValue("@Email", email);

            con.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
