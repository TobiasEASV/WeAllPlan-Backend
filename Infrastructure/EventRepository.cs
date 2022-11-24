using Application;
using Application.Interfaces;
using Core;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure;

public class EventRepository : IEventRepository
{
    private DBContextSqlite _dbContextSqlite;
    public EventRepository(DBContextSqlite dbContextSqlite)
    {
        _dbContextSqlite = dbContextSqlite;
    }
    public async Task<Event> CreateEvent(Event anEvent)
    {
        EntityEntry<Event> x = await _dbContextSqlite.Events.AddAsync(anEvent);
        await _dbContextSqlite.SaveChangesAsync();
        return x.Entity;
    }

    public async Task<List<Event>> GetAll()
    {
        return await _dbContextSqlite.Events.ToListAsync();
    }
}