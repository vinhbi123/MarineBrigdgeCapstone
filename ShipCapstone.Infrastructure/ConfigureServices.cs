using System.Text.Json.Serialization;
using Garage.Domain.Models.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShipCapstone.Domain.Models.Settings;
using ShipCapstone.Infrastructure.Configurations;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories;
using ShipCapstone.Infrastructure.Repositories.Interface;
using StackExchange.Redis;

namespace ShipCapstone.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ShipCapstoneContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString"),
                builder => builder.MigrationsAssembly(typeof(ShipCapstoneContext).Assembly.FullName));
        });
        services.AddScoped<IUnitOfWork<ShipCapstoneContext>, UnitOfWork<ShipCapstoneContext>>();
        services.AddRedis(configuration);
        services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.Configure<OAuthSettings>(configuration.GetSection("OAuthSettings"));
        services.Configure<S3CompatibleStorageSettings>(configuration.GetSection("S3CompatibleStorageSettings"));
        services.AddScoped<ShipCapstoneSeed>();
        services.AddJwt(configuration);
        services.AddSwagger();
        services.AddCors();
        services.AddControllers().AddJsonOptions(x =>
        {
            x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            // x.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
        });
        return services;
    }
    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetConnectionString("Redis");
        if (redisConnectionString != null)
            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(redisConnectionString));
        return services;
    }
}