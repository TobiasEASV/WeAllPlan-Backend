using Application.Interfaces;
using Infrastructure.DB;

namespace Infrastructure;

public class EventSlotRepository: IEventSlotRepository
{
    private DBContextSqlite _dbContextSqlite;

    public EventSlotRepository(DBContextSqlite dbContextSqlite)
    {
        _dbContextSqlite = dbContextSqlite;
    }
}