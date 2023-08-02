using Foundation.Dtos;
using Foundation.Web;
using Microsoft.EntityFrameworkCore;
using Web.Database.Models;
using Web.Controllers;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var mapperConfig = new MapperConfiguration(
                cfg =>
                {
                    cfg.AllowNullCollections = true;
                    cfg.AddProfile<DtoEntityProfile>();
                });
var mapper = mapperConfig.CreateMapper();

builder.Services.AddSingleton(mapper);
builder.Services.AddControllers();
builder.Services.AddManager();
builder.Services.AddRepositories();
builder.Services.AddDbContext<BaseContext>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFoundationWeb();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
