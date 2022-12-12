using Application.Interfaces;
using Core;
using Infrastructure.DB;
using Infrastructure.DBPostgresql;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class UserRepository: IUserRepository
{
    private readonly DBContextPostgresql _context;

    public UserRepository(DBContextPostgresql context)
    {
        _context = context;
    }
    public async Task<User> GetUserByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.Email == email) ?? throw new KeyNotFoundException("Invalid login");
    }

    public async Task CreateNewUser(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}