using PersonalPages.Models;

public interface IUserService
{
    User GetProfile(string email);
    void UpdateProfile(string email, UpdateUserDto dto);
}
