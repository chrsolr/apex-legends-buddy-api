global using apex_legends_buddy_api.Services;
global using apex_legends_buddy_api.Models;
global using apex_legends_buddy_api.Shared;
global using apex_legends_buddy_api.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddHttpClient<GamepediaService>();
builder.Services.AddHttpClient<ApexTrackerService>();

// Services/Repository
builder.Services
    .AddScoped<IGamepediaService, GamepediaService>()
    .AddScoped<IApexTrackerService, ApexTrackerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
