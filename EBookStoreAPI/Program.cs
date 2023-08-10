using EBookStoreAPI.Models;
using EBookStoreAPI.Models.EFModels;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the DI container.
string EBookStoreConnectionString = builder.Configuration.GetConnectionString("EBookStore");
builder.Services.AddDbContext<EBookStoreContext>(options =>
{
	options.UseSqlServer(EBookStoreConnectionString);
});

builder.Services.AddControllers();

builder.Services.AddCors(options => {
	options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
