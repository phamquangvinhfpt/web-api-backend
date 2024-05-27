using Core.Infrastructure;
using Hangfire;
using Serilog;
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
try
{
    Log.Information("starting server.");
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddControllers();
    builder.Host.UseSerilog((context, logger) =>
    {
        logger.MinimumLevel.Warning();
        logger.WriteTo.Console();
        logger.ReadFrom.Configuration(context.Configuration);
    });
    builder.Services.AddAutoMapper(typeof(Program));
    builder.Services.AddInfrastructure(builder.Configuration);

    var app = builder.Build();
    Startup.Initialize(app.Services, app.Configuration);

    // app.MapIdentityApi<AppUser>();
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