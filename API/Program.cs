using System.Text.Json.Serialization;
using Application.Helpers;
using Infrastructure.DB;
using Infrastructure.DBPostgresql;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Setup DependencyResolver Service
Application.DependencyResolver.DependencyResolverService.RegisterApplicationLayer(builder.Services);
Infrastructure.DependencyResolver.DependencyResolverService.RegisterInfrastructureLayer(builder.Services);

if (builder.Environment.IsDevelopment())
{
    /* Setup the Database Context Class with ConnectionString in AppSettings */
   
    builder.Services.AddDbContext<DBContextSqlite>(options => options.UseSqlite(
       builder.Configuration.GetConnectionString("ConnectionStringsDevelopment")));
}

if (builder.Environment.IsProduction())
{
    builder.Services.AddDbContext<DBContextPostgresql>(options => options.UseNpgsql(
        builder.Configuration.GetConnectionString("ConnectionStringsProduction")));
}




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsProduction())
{
    
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();