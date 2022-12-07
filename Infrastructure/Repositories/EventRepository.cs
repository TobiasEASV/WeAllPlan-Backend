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
        List<EventSlot> eventSlots = new List<EventSlot>();
        EntityEntry<Event> x = await _dbContextSqlite.Events.AddAsync(anEvent);
        foreach (var eventSlot in anEvent.EventSlots)
        {
            eventSlot.Event = x.Entity;
            eventSlots.Add(eventSlot);
        }

        await _dbContextSqlite.EventSlots.AddRangeAsync(eventSlots);
        await _dbContextSqlite.SaveChangesAsync();
        return x.Entity;
    }

    public async Task<List<Event>> GetAll()
    {
        return await _dbContextSqlite.Events.Include(e => e.User).ToListAsync();
    }

    public async Task<Event> UpdateEvent(Event Event, int userId)
    {
        await Task.Run(() => _dbContextSqlite.Events.Attach(Event));
        _dbContextSqlite.Entry(Event).Property(e => e.Title).IsModified = true;
        _dbContextSqlite.Entry(Event).Property(e => e.Description).IsModified = true;
        _dbContextSqlite.Entry(Event).Property(e => e.Location).IsModified = true;
        await _dbContextSqlite.SaveChangesAsync();
        return Event;
    }

    public void Delete(Event Event)
    {
        _dbContextSqlite.Events.Remove(Event);
        _dbContextSqlite.SaveChanges();
    }

    public User getUser(int userId)
    {
       return _dbContextSqlite.Users.Find(userId);
    }

    public async Task<Event> GetEventById(int id)
    {
        return await _dbContextSqlite.Events.Include(e => e.User).Include(e => e.EventSlots).SingleAsync<Event>( e=> e.Id == id);
    }

    public async Task<List<Event>> GetEventByUserId(int userId)
    {
        return await _dbContextSqlite.Events.Include(e => e.User).Where(e => e.User.Id == userId).ToListAsync();
    }
}