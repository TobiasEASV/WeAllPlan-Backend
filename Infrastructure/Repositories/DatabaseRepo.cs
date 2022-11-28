using Infrastructure.DB;

namespace Infrastructure;

public class DatabaseRepo
{
    private readonly DBContextSqlite _context;

    public DatabaseRepo(DBContextSqlite context)
    {
        _context = context;
    }
    
    public void CreateDB()
    {
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }
}