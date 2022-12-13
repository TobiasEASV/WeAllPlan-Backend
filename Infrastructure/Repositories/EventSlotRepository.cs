using Application.Interfaces;
using Core;
using Infrastructure.DBPostgresql;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class EventSlotRepository : IEventSlotRepository
{
    private DBContextPostgresql _Context;

    public EventSlotRepository(DBContextPostgresql context)
    {
        _Context = context;
    }

    public async void CreateEventSlot(List<EventSlot> validatedEventSlots)
    {
        await _Context.EventSlots.AddRangeAsync(validatedEventSlots);
        await _Context.SaveChangesAsync();
    }

    public async Task<List<EventSlot>> GetAll()
    {
        return await _Context.EventSlots.AsNoTracking()
            .Include(u => u.Event.User)
            .Include(u => u.SlotAnswers)
            .ToListAsync();
    }

    public async void UpdateEventSlot(List<EventSlot> updateList)
    {
        foreach (var eventSlot in updateList)
        {
            _Context.EventSlots.Attach(eventSlot);
            _Context.Entry(eventSlot).Property(e => e.Confirmed).IsModified = true;
            _Context.Entry(eventSlot).Property(e => e.Id).IsModified = false;
            _Context.Entry(eventSlot).Property(e => e.EndTime).IsModified = true;
            _Context.Entry(eventSlot).Property(e => e.StartTime).IsModified = true;
        }

        await _Context.SaveChangesAsync();
    }

    public async void DeleteEventSlot(List<EventSlot> eventSlotList)
    {
        _Context.EventSlots.RemoveRange(eventSlotList);
        await _Context.SaveChangesAsync();
    }

    public async Task<Event> getEventFromId(int eventId)
    {
        return await _Context.Events.Include(e => e.User).FirstAsync(e => e.Id == eventId);
    }
}