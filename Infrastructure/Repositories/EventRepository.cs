using Application.Interfaces;
using Core;
using Infrastructure.DBPostgresql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure;

public class EventRepository : IEventRepository
{
    private DBContextPostgresql _Context;

    public EventRepository(DBContextPostgresql context)
    {
        _Context = context;
    }

    public async Task<Event> CreateEvent(Event anEvent)
    {
        EntityEntry<Event> x = await _Context.Events.AddAsync(anEvent);
        await _Context.SaveChangesAsync();
        return x.Entity;
    }

    public async Task<List<Event>> GetAll()
    {
        return await _Context.Events.Include(e => e.User).ToListAsync();
    }

    public async Task<Event> UpdateEvent(Event Event)
    {
        await Task.Run(() => _Context.Events.Attach(Event));
        _Context.Entry(Event).Property(e => e.Title).IsModified = true;
        _Context.Entry(Event).Property(e => e.Description).IsModified = true;
        _Context.Entry(Event).Property(e => e.Location).IsModified = true;
        await _Context.SaveChangesAsync();
        return Event;
    }

    public void Delete(Event Event)
    {
        _Context.Events.Remove(Event);
        _Context.SaveChanges();
    }

    public User getUser(int userId)
    {
        return _Context.Users.Find(userId);
    }

    public async Task<Event> GetEventById(int id)
    {
        return await _Context.Events.Include(Event => Event.User)
            .Include(Event => Event.EventSlots)!
            .ThenInclude<Event, EventSlot, List<SlotAnswer>>(EventSlot => EventSlot.SlotAnswers)
            .SingleAsync<Event>(Event => Event.Id == id);
    }

    public async Task<List<Event>> GetEventByUserId(int userId)
    {
        return await _Context.Events.Include(e => e.User).Where(e => e.User.Id == userId).ToListAsync();
    }
}