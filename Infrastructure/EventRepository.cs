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

    public async Task<Event> UpdateEvent(Event Event)
    {
        await Task.Run(() => _dbContextSqlite.Attach(Event));
        _dbContextSqlite.Entry(Event).Property(e => e.Title).IsModified = true;
        _dbContextSqlite.Entry(Event).Property(e => e.Description).IsModified = true;
        _dbContextSqlite.Entry(Event).Property(e => e.Location).IsModified = true;
        _dbContextSqlite.SaveChanges();
        return Event;
    }

    public void Delete(Event Event)
    {
        _dbContextSqlite.Events.Remove(Event);
        _dbContextSqlite.SaveChanges();
    }
}