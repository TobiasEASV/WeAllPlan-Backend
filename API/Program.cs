using System.Text;
using System.Text.Json.Serialization;
using Application;
using Application.DTO;
using AutoMapper;
using Core;
using FluentValidation;
using Infrastructure;
using Application.Helpers;
using Infrastructure.DB;
using Infrastructure.DBPostgresql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mapper = new MapperConfiguration(config =>
{
    config.CreateMap<EventSlotDTO, EventSlot>();
    config.CreateMap<EventSlot, EventSlotDTO>();
    config.CreateMap<EventDTO, Event>();
    config.CreateMap<Event,EventDTO>();
    config.CreateMap<SlotAnswerDTO, SlotAnswer>();
    config.CreateMap<SlotAnswer, SlotAnswerDTO>();
}).CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration.GetValue<string>("AppSettings:Secret")))
    };
});


builder.Services.AddCors();


// Setup DependencyResolver Service
Application.DependencyResolver.DependencyResolverService.RegisterApplicationLayer(builder.Services);
Infrastructure.DependencyResolver.DependencyResolverService.RegisterInfrastructureLayer(builder.Services);

builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());



var uriString = builder.Configuration.GetConnectionString("ConnectionStringsDevelopment");
var uri = new Uri(uriString);
var db = uri.AbsolutePath.Trim('/');
var user = uri.UserInfo.Split(':')[0];
var passwd = uri.UserInfo.Split(':')[1];
var port = uri.Port > 0 ? uri.Port : 5432;
var connStr = string.Format("Server={0};Database={1};User Id={2};Password={3};Port={4}",
    uri.Host, db, user, passwd, port);


if (builder.Environment.IsDevelopment())
{
    /* Setup the Database Context Class with ConnectionString in AppSettings */
   
    builder.Services.AddDbContext<DBContextPostgresql>(options => options.UseNpgsql(connStr));
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

app.UseCors(options =>
{
    options.SetIsOriginAllowed(option => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
});


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

