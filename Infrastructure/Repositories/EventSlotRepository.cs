using Application.Interfaces;
using Core;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class EventSlotRepository : IEventSlotRepository
{
    private DBContextSqlite _dbContextSqlite;

    public EventSlotRepository(DBContextSqlite dbContextSqlite)
    {
        _dbContextSqlite = dbContextSqlite;
    }

    public async void CreateEventSlot(List<EventSlot> validatedEventSlots)
    {
        await _dbContextSqlite.EventSlots.AddRangeAsync(validatedEventSlots);
        await _dbContextSqlite.SaveChangesAsync();
    }

    public async Task<List<EventSlot>> GetAll()
    {
        return await _dbContextSqlite.EventSlots.AsNoTracking()
            .Include(u => u.Event.User)
            .Include(u => u.SlotAnswers)
            .ToListAsync();
    }

    public async void UpdateEventSlot(List<EventSlot> updateList)
    {
        foreach (var eventSlot in updateList)
        {
            _dbContextSqlite.EventSlots.Attach(eventSlot);
            _dbContextSqlite.Entry(eventSlot).Property(e => e.Confirmed).IsModified = true;
            _dbContextSqlite.Entry(eventSlot).Property(e => e.Id).IsModified = false;
            _dbContextSqlite.Entry(eventSlot).Property(e => e.EndTime).IsModified = true;
            _dbContextSqlite.Entry(eventSlot).Property(e => e.StartTime).IsModified = true;
        }

        await _dbContextSqlite.SaveChangesAsync();
    }

    public async void DeleteEventSlot(List<EventSlot> eventSlotList)
    {
        _dbContextSqlite.EventSlots.RemoveRange(eventSlotList);
        await _dbContextSqlite.SaveChangesAsync();
    }

    public async Task<Event> getEventFromId(int eventId)
    {
        return await _dbContextSqlite.Events.Include(e => e.User).FirstAsync(e => e.Id == eventId);
    }
}