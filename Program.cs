using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DB"));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddHttpClient<GamepediaService>();
builder.Services.AddHttpClient<ApexTrackerService>();
builder
    .Services.AddScoped<IGamepediaService, GamepediaService>()
    .AddScoped<IApexTrackerService, ApexTrackerService>()
    .AddScoped<ILegendService, LegendService>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
