using System.Text.Json.Serialization;
using Application;
using AutoMapper;
using Core;
using FluentValidation;
using Infrastructure;
using Infrastructure.DB;
using Infrastructure.DBPostgresql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mapper = new MapperConfiguration(config =>
{
    config.CreateMap<EventDTO, Event>();
    config.CreateMap<Event,EventDTO>();
    config.CreateMap<SlotAnswerDTO, SlotAnswer>();
    config.CreateMap<SlotAnswer, SlotAnswerDTO>();
}).CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


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

