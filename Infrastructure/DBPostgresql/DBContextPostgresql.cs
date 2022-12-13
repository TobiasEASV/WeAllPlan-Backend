using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.DBPostgresql;

public class DBContextPostgresql: DbContext
{


    public DBContextPostgresql()
    {

    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(ConnStr.Get());
    }
    

    public DbSet<Event> Events { get; set; }
    public DbSet<EventSlot> EventSlots { get; set; }
    public DbSet<SlotAnswer> SlotAnswers { get; set; }
    public DbSet<User> Users { get; set; }
}

public class ConnStr
{
    public static string Get()
    {
        var uriString = "postgres://pjevkptg:XZ32tI7LXAGoZZcK611xCwhub1jLDe95@mouse.db.elephantsql.com/pjevkptg";
        var uri = new Uri(uriString);
        var db = uri.AbsolutePath.Trim('/');
        var user = uri.UserInfo.Split(':')[0];
        var passwd = uri.UserInfo.Split(':')[1];
        var port = uri.Port > 0 ? uri.Port : 5432;
        var connStr = string.Format("Server={0};Database={1};User Id={2};Password={3};Port={4}",
            uri.Host, db, user, passwd, port);
        return connStr;
    }
}
