using FintechDashboard.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient<StockService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
