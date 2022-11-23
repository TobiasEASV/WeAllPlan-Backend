using Application.Interfaces;
using Core;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class UserRepository: IUserRepository
{
    private readonly DBContextSqlite _context;

    public UserRepository(DBContextSqlite context)
    {
        _context = context;
    }
    public User GetUserByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public void CreateNewUser(User user)
    {
        throw new NotImplementedException();
    }
}