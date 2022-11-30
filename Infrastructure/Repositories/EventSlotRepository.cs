﻿using Application.Interfaces;
using Core;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;


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
        return await _dbContextSqlite.EventSlots
            .Include( u=> u.Event)
            .Include(u =>u.SlotAnswers)
            .ToListAsync();
    }

    public async void UpdateEventSlot(List<EventSlot> updateList)
    {
        _dbContextSqlite.EventSlots.AttachRange(updateList);
        await _dbContextSqlite.SaveChangesAsync();

    }

    public async void DeleteEventSlot(List<EventSlot> eventSlotList)
    {
        _dbContextSqlite.EventSlots.RemoveRange(eventSlotList);
        await _dbContextSqlite.SaveChangesAsync();
    }

    public async Task<Event> getEventFromId(int eventId)
    {
        return await _dbContextSqlite.Events.FindAsync(eventId);
    }
}