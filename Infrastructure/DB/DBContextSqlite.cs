using Core;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DB;

public class DBContextSqlite:DbContext
{
    public DBContextSqlite(DbContextOptions<DBContextSqlite> options) : base(options)
    {
        
    }
    public DbSet<Event> Events { get; set; }
    public DbSet<EventSlot> EventSlots { get; set; }
    public DbSet<SlotAnswer> SlotAnswers { get; set; }
    public DbSet<User> Users { get; set; }

}