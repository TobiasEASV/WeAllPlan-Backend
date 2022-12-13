using Application.Interfaces;
using Core;
using Infrastructure.DBPostgresql;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class UserRepository : IUserRepository
{
    private readonly DBContextPostgresql _Context;

    public UserRepository(DBContextPostgresql context)
    {
        _Context = context;
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _Context.Users.FirstOrDefaultAsync(user => user.Email == email) ??
               throw new KeyNotFoundException("Invalid login");
    }

    public async Task CreateNewUser(User user)
    {
        await _Context.Users.AddAsync(user);
        await _Context.SaveChangesAsync();
    }
}