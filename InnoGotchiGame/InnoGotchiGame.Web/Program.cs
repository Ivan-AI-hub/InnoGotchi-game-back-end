using InnoGotchiGame.Application.Filtrators;
using InnoGotchiGame.Application.Filtrators.Base;
using InnoGotchiGame.Application.Managers;
using InnoGotchiGame.Application.Sorters;
using InnoGotchiGame.Application.Sorters.Base;
using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence.Interfaces;
using InnoGotchiGame.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddTransient<Filtrator<User>, UserFiltrator>();
builder.Services.AddTransient<Sorter<User>, UserSorter>();
builder.Services.AddTransient<IRepository<User>, UserRepository>();

builder.Services.AddTransient<UserManager>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
