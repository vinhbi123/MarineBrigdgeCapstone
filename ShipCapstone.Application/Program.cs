using Serilog;
using ShipCapstone.Application.Common.Extensions;
using ShipCapstone.Application.Common.Middlewares;
using ShipCapstone.Infrastructure;
using ShipCapstone.Infrastructure.Configurations;
using ShipCapstone.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(SerilogConfig.Configure);
Log.Information("Starting Marine Bridge API up");

try
{
    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddApplicationServices(builder.Configuration);

    var app = builder.Build();

    app.UseDefaultFiles();
    app.UseStaticFiles();
    if (app.Environment.IsDevelopment() || app.Environment.IsProduction() || app.Environment.IsStaging())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Marine Bridge API V1");
            c.InjectStylesheet("/assets/css/kkk.css");
        });
    }

    // using (var scope = app.Services.CreateScope())
    // {
    //     try
    //     {
    //         var shipCapstoneSeed = scope.ServiceProvider.GetRequiredService<ShipCapstoneSeed>();
    //         await shipCapstoneSeed.InitializeAsync();
    //     }
    //     catch (Exception e)
    //     {
    //         Log.Error(e, "An error occurred while seeding the database.");
    //         throw;
    //     }
    // }

    app.UseCors(builder =>
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
    app.UseMiddleware<GlobalException>();
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }

    Log.Fatal(ex, $"Unhandled: {ex.Message}");
}
finally
{
    Log.Information("Shut down Marine Bridge API complete");
    Log.CloseAndFlush();
}