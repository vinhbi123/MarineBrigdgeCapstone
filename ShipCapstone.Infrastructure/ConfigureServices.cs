using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShipCapstone.Infrastructure.Configurations;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories;
using ShipCapstone.Infrastructure.Repositories.Interface;

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
}