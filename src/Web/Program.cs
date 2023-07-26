using Foundation.Dtos;
using Foundation.Web;
using Microsoft.EntityFrameworkCore;
using Web.Database.Models;
using Web.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var services = builder.Services;
services.AddDbContext<BaseContext<CategoryDto>>(opt =>
opt.UseInMemoryDatabase("CategoryList"));
services.AddDbContext<BaseContext<ProductDto>>(opt =>
opt.UseInMemoryDatabase("ProductList"));
services.AddTransient<DefaultRepository<CategoryDto>>();
services.AddTransient<DefaultRepository<ProductDto>>();


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
