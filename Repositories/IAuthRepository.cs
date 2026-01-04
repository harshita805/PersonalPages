using PersonalPages.Models;

namespace PersonalPages.Repositories
{
    public interface IAuthRepository
    {
        bool UserExists(string email);
        void Register(User user);
        string GetPasswordHash(string email);
    }
}
