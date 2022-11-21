using Core;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DBPostgresql;

public class DBContextPostgresql: DbContext
{
    public DBContextPostgresql(DbContextOptions<DBContextPostgresql> options) : base(options)
    {
    }
    
    DbSet<Event> Events { get; set; }
    DbSet<EventSlot> EventSlots { get; set; }
    DbSet<SlotAnswer> SlotAnswers { get; set; }
    DbSet<User> Users { get; set; }
}