using Core.Infrastructure;
using Core.Infrastructure.Notifications;
using Hangfire;
using Serilog;
using init = Core.Infrastructure.Startup;
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
try
{
    Log.Information("starting server.");
    var builder = WebApplication.CreateBuilder(args);
    if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Files")))
    {
        Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Files"));
    }
    builder.Services.AddControllers();
    builder.Host.UseSerilog((context, logger) =>
    {
        logger.MinimumLevel.Warning();
        logger.WriteTo.Console();
        logger.ReadFrom.Configuration(context.Configuration);
    });
    builder.Services.AddAutoMapper(typeof(Program));
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddAutoMapper(typeof(MappingProfile));
    var app = builder.Build();
    init.Initialize(app.Services, app.Configuration);

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
            c.RoutePrefix = string.Empty;
        });
    }
    app.UseInfrastructure();
    app.MapControllers();
    app.MapNotifications();
    app.MapHangfireDashboard();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "server terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}