using TavernSystem.Application;
using TavernSystem.Models.DTO;
using TaverSystem.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Database");

builder.Services.AddTransient<ITavernRepository, TavernRepository>(s => new TavernRepository(connectionString));
builder.Services.AddTransient<ITavernService, TavernService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/adventurers", async (ITavernService service) =>
{
    try
    {
        List<object> result = await service.GetAllTravelersAsync();
        return result.Count == 0 ? Results.NotFound() : Results.Ok(result);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/api/adventurers/{id}", async (int id, ITavernService service) =>
{
    try
    {
        var result = await service.GetTravellerByIdAsync(id);
        return result is not null ? Results.Ok(result) : Results.NotFound();
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapPost("/api/adventurers", async (AdventurerDTO request, ITavernService service) =>
{
    try
    {
        var result = await service.AddTravelerAsync(request);
        return result ? Results.NoContent() : Results.Forbid();
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.Run();
