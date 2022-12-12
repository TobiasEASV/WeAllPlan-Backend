using Application.Interfaces;
using Core;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class SlotAnswerRepository : ISlotAnswerRepository
{
    private DBContextSqlite _dbContextSqlite;

    public SlotAnswerRepository(DBContextSqlite dbContextSqlite)
    {
        _dbContextSqlite = dbContextSqlite;
    }

    public async Task CreateSlotAnswer(List<SlotAnswer> slotAnswer)
    {
        await _dbContextSqlite.SlotAnswers.AddRangeAsync(slotAnswer);
        await _dbContextSqlite.SaveChangesAsync();
    }

    public async Task<List<SlotAnswer>> GetAll()
    {
        return await _dbContextSqlite.SlotAnswers.Include(s => s.EventSlot.Event.User).ToListAsync();
    }

    public async Task<SlotAnswer> UpdateSlotAnswer(SlotAnswer slotAnswer)
    {
        await Task.Run(() => _dbContextSqlite.SlotAnswers.Attach(slotAnswer));
        _dbContextSqlite.Entry(slotAnswer).Property(s => s.Answer).IsModified = true;
        _dbContextSqlite.Entry(slotAnswer).Property(s => s.Email).IsModified = false;
        _dbContextSqlite.Entry(slotAnswer).Property(s => s.Id).IsModified = false;
        _dbContextSqlite.Entry(slotAnswer).Property(s => s.UserName).IsModified = false;
        await _dbContextSqlite.SaveChangesAsync();
        return slotAnswer;
    }

    public async void DeleteSlotAnswers(List<SlotAnswer> listToDelete)
    {
        _dbContextSqlite.SlotAnswers.RemoveRange(listToDelete);
        await _dbContextSqlite.SaveChangesAsync();
    }

    public async Task<EventSlot> getEventSlot(int eventSlotId)
    {
        return await _dbContextSqlite.EventSlots.FindAsync(eventSlotId);
    }
}