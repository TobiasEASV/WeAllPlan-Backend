using Application;
using Application.Interfaces;
using Core;
using Infrastructure.DB;

namespace Infrastructure;

public class EventRepository : IEventRepository
{
    private DBContextSqlite _dbContextSqlite;
    public EventRepository(DBContextSqlite dbContextSqlite)
    {
        _dbContextSqlite = dbContextSqlite;
    }
    public Event CreateEvent(Event anEvent)
    {
        _dbContextSqlite.Events.Add(anEvent);
        _dbContextSqlite.SaveChanges();
        return anEvent;
    }
}