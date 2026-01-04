using PersonalPages.Models;

namespace PersonalPages.Repositories
{
    public interface IUserRepository
    {
        User GetUserByEmail(string email);
        void UpdateUser(string email, UpdateUserDto dto);
    }
}
