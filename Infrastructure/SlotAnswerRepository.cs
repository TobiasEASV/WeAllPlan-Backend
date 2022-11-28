using Application.Interfaces;
using AutoMapper;
using Core;
using Infrastructure.DB;
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
}