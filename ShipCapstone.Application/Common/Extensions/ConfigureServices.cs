using Mediator;
using Microsoft.AspNetCore.Mvc;
using ShipCapstone.Application.Common.Behaviours;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Domain.Models.Common;

namespace ShipCapstone.Application.Common.Extensions;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddMediator( options =>
            {
                options.Namespace = "ShipCapstone.Application.Controllers";
                options.ServiceLifetime = ServiceLifetime.Scoped;
            })
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddScoped(typeof(ValidationUtil<>));
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors.Select(e => e.ErrorMessage))
                    .ToList();

                var apiResponse = new ApiResponse()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "Data validation error",
                    Data = errors
                };

                return new BadRequestObjectResult(apiResponse);
            };
        });
        return services;
    }
}