using PersonalPages.Models;
using PersonalPages.Repositories;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;

    public UserService(IUserRepository repo)
    {
        _repo = repo;
    }

    public User GetProfile(string email)
    {
        return _repo.GetUserByEmail(email);
    }

    public void UpdateProfile(string email, UpdateUserDto dto)
    {
        _repo.UpdateUser(email, dto);
    }
}
