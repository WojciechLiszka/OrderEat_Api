using OrderEat.Api.MiddleWare;
using OrderEat.Application.Extensions;
using OrderEat.Infrastructure.Extensions;
using OrderEat.Infrastructure.Seeders;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ErrorHandlingMiddleware>();

builder.Services.AddControllers().AddFluentValidation();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var scope = app.Services.CreateScope();

var seeder = scope.ServiceProvider.GetRequiredService<OrderEatSeeder>();

await seeder.Seed();

app.UseMiddleware<ErrorHandlingMiddleware>();
try
{
    // Kod generuj¹cy Swaggera
    app.UseSwagger();
    app.UseSwaggerUI();
}
catch (Exception ex)
{
    var exceptionDetails = $"{ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}";
    Console.WriteLine(exceptionDetails );
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{ }