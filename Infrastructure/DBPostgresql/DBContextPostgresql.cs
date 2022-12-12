using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.DependencyResolution;
using Core;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace Infrastructure.DBPostgresql;


[DbConfigurationType(typeof(ElephantSqlDbConfiguration))]
public class DBContextPostgresql: DbContext
{
    public DBContextPostgresql(DbContextOptions<DBContextPostgresql> options) : base(options)
    {
    }
    
    public Microsoft.EntityFrameworkCore.DbSet<Event> Events { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<EventSlot> EventSlots { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<SlotAnswer> SlotAnswers { get; set; }
    public Microsoft.EntityFrameworkCore.DbSet<User> Users { get; set; }
}

internal sealed class ElephantSqlDbConfiguration : DbConfiguration
{
    private const string ManifestToken = @"9.2.8";
    public ElephantSqlDbConfiguration()
    {
        this.AddDependencyResolver(new SingletonDependencyResolver<IManifestTokenResolver>(new ManifestTokenService()));
    }
    private sealed class ManifestTokenService : IManifestTokenResolver
    {
        private static readonly IManifestTokenResolver DefaultManifestTokenResolver
            = new DefaultManifestTokenResolver();
        public string ResolveManifestToken(DbConnection connection)
        {
            if (connection is NpgsqlConnection)
            {
                return ManifestToken;
            }
            return DefaultManifestTokenResolver.ResolveManifestToken(connection);
        }
    }
}