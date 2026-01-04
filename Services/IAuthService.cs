using PersonalPages.Models;

namespace PersonalPages.Services
{
    public interface IAuthService
    {
        void Register(RegisterDto dto);
        string Login(LoginDto dto);
    }
}
