using Application.Interfaces;
using AutoMapper;
using Core;
using Infrastructure.DB;

namespace Infrastructure;

public class SlotAnswerRepository : ISlotAnswerRepository
{
    
    private DBContextSqlite _dbContextSqlite;
    public SlotAnswerRepository(DBContextSqlite dbContextSqlite)
    {
        _dbContextSqlite = dbContextSqlite;
    }

    public void CreateSlotAnswer(SlotAnswer isAny)
    {
        throw new NotImplementedException();
    }
}