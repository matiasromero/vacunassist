global using VacunassistBackend.Data;
using Microsoft.EntityFrameworkCore;
using VacunassistBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Adding My Dependencies
builder.Services.AddTransient<IUsersService, UsersService>();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();