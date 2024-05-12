using Core.Infrastructure.Exceptions;
using Core.Infrastructure.Serilog;
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
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddTransient<IDummyService, DummyService>();
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();
    var app = builder.Build();
    app.MapGet("/", (IDummyService svc) => svc.DoSomething());
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseHttpsRedirection();
    app.UseMiddleware<ErrorHandlerMiddleware>();
    app.UseExceptionHandler();
    app.UseAuthorization();
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