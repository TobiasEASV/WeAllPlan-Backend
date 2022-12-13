using Application.Interfaces;
using Core;
using Infrastructure.DBPostgresql;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class SlotAnswerRepository : ISlotAnswerRepository
{
    private DBContextPostgresql _Context;

    public SlotAnswerRepository(DBContextPostgresql context)
    {
        _Context = context;
    }

    public async Task CreateSlotAnswer(List<SlotAnswer> slotAnswer)
    {
        await _Context.SlotAnswers.AddRangeAsync(slotAnswer);
        await _Context.SaveChangesAsync();
    }

    public async Task<List<SlotAnswer>> GetAll()
    {
        return await _Context.SlotAnswers.Include(s => s.EventSlot.Event.User).ToListAsync();
    }

    public async Task<SlotAnswer> UpdateSlotAnswer(SlotAnswer slotAnswer)
    {
        await Task.Run(() => _Context.SlotAnswers.Attach(slotAnswer));
        _Context.Entry(slotAnswer).Property(s => s.Answer).IsModified = true;
        _Context.Entry(slotAnswer).Property(s => s.Email).IsModified = false;
        _Context.Entry(slotAnswer).Property(s => s.Id).IsModified = false;
        _Context.Entry(slotAnswer).Property(s => s.UserName).IsModified = false;
        await _Context.SaveChangesAsync();
        return slotAnswer;
    }

    public async void DeleteSlotAnswers(List<SlotAnswer> listToDelete)
    {
        _Context.SlotAnswers.RemoveRange(listToDelete);
        await _Context.SaveChangesAsync();
    }

    public async Task<EventSlot> getEventSlot(int eventSlotId)
    {
        return await _Context.EventSlots.FindAsync(eventSlotId);
    }
}