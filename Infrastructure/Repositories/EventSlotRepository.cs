using Application.Interfaces;
using Core;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure;

public class EventSlotRepository: IEventSlotRepository
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
        return await _dbContextSqlite.EventSlots.ToListAsync();
    }

    public Task<List<EventSlot>> UpdateEvent(List<EventSlot> eventSlots)
    {
        throw new NotImplementedException();
    }
}