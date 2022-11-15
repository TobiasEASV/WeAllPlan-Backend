using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DB;

public class DBContext:DbContext
{
    public DBContext(DbContextOptions<DBContext> options) : base(options)
    {
    }
    
    //DbSet<>

}