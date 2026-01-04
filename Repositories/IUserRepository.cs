using PersonalPages.Models;

namespace PersonalPages.Repositories
{
    public interface IUserRepository
    {
        bool UserExists(string email);
        void Register(User user);
        string GetPasswordHash(string email);
    }
}
