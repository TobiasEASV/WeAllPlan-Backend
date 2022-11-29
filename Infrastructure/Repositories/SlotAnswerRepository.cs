using System.Net.Sockets;
using Application.Interfaces;
using AutoMapper;
using Core;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure;

public class SlotAnswerRepository : ISlotAnswerRepository
{
    
    private DBContextSqlite _dbContextSqlite;
    public SlotAnswerRepository(DBContextSqlite dbContextSqlite)
    {
        _dbContextSqlite = dbContextSqlite;
    }

    public async Task<SlotAnswer> CreateSlotAnswer(SlotAnswer slotAnswer)
    {
        EntityEntry<SlotAnswer> x = await _dbContextSqlite.SlotAnswers.AddAsync(slotAnswer);
        await _dbContextSqlite.SaveChangesAsync();
        return x.Entity;
    }

    public async Task<List<SlotAnswer>> GetAll()
    {
        return await _dbContextSqlite.SlotAnswers.ToListAsync();
    }

    public async Task<SlotAnswer> UpdateSlotAnswer(SlotAnswer slotAnswer)
    {
        await Task.Run(() => _dbContextSqlite.SlotAnswers.Attach(slotAnswer));
        _dbContextSqlite.Entry(slotAnswer).Property(s => s.Answer).IsModified = true;
        _dbContextSqlite.Entry(slotAnswer).Property(s => s.Email).IsModified = false;
        _dbContextSqlite.Entry(slotAnswer).Property(s => s.Id).IsModified = false;
        _dbContextSqlite.Entry(slotAnswer).Property(s => s.UserName).IsModified = false;
        _dbContextSqlite.Entry(slotAnswer).Property(s => s.EventSlot).IsModified = false;
        await _dbContextSqlite.SaveChangesAsync();
        return slotAnswer;
    }

    public async void DeleteSlotAnswers(List<SlotAnswer> listToDelete)
    {
        _dbContextSqlite.SlotAnswers.RemoveRange(listToDelete);
        await _dbContextSqlite.SaveChangesAsync();
    }
}