using Core;

namespace Application.Interfaces;

public interface IUserRepository
{
    public Task<User> GetUserByEmail(string email);
    public Task CreateNewUser(User user);
}