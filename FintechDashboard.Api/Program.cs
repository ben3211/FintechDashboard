using FintechDashboard.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:7096")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddHttpClient<StockService>();
builder.Services.AddHttpClient<NewsService>();
builder.Services.AddHttpClient<OpenAiService>();


var app = builder.Build();

app.UseHttpsRedirection();


app.UseCors();


app.MapControllers();

app.Run();
