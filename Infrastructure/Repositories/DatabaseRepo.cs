using Infrastructure.DB;
using Infrastructure.DBPostgresql;

namespace Infrastructure;

public class DatabaseRepo
{
    //private readonly DBContextSqlite _context;
    private readonly DBContextPostgresql _context;

    public DatabaseRepo(DBContextPostgresql context)
    {
        _context = context;
    }
    
    public void CreateDB()
    {
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }
}