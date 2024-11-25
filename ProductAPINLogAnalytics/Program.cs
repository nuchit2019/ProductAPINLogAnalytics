using NLog;
using NLog.Extensions.Logging;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using ProductAPINLogAnalytics.Services;

var logger = LogManager.Setup()
    .LoadConfigurationFromFile("nlog.config") // Load NLog configuration from file
    .GetCurrentClassLogger();

try
{

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container. 
    builder.Services.AddSingleton<ILogAnalyticsService,LogAnalyticsService>();

    builder.Services.AddControllers();

    // Configure logging
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(LogLevel.Information);
    builder.Logging.AddNLog(); // Use NLog as

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
    app.UseAuthorization();
    app.MapControllers(); 
    app.Run();

}
catch (Exception ex)
{
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown(); // Ensure to flush and stop any NLog targets
}