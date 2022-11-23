using Core;

namespace Application.Interfaces;

public interface IUserRepository
{
    public User GetUserByEmail(string email);
    public void CreateNewUser(User user);
}