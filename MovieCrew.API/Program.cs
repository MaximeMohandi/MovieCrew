using Microsoft.EntityFrameworkCore;
using MovieCrew.API.Extension;
using MovieCrew.Core.Data;

var builder = WebApplication.CreateBuilder(args);

// Add JWT Authentication
builder.Services.ConfigureAuthentication(builder.Configuration);

// connect to database
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Inject domain services
builder.Services.InjectDomainServices();

builder.Services.AddControllers();
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

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
} // For test purpose
