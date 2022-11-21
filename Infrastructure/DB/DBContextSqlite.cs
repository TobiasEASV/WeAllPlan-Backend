using Core;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DB;

public class DBContextSqlite:DbContext
{
    public DBContextSqlite(DbContextOptions<DBContextSqlite> options) : base(options)
    {
        
    }
    DbSet<Event> Events { get; set; }
    DbSet<EventSlot> EventSlots { get; set; }
    DbSet<SlotAnswer> SlotAnswers { get; set; }
    DbSet<User> Users { get; set; }

}