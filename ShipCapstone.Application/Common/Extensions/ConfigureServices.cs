using FluentValidation;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using ShipCapstone.Application.Common.Behaviours;
using ShipCapstone.Application.Common.Utils;
using ShipCapstone.Application.Features.Accounts.Command.ChangePassword;
using ShipCapstone.Application.Features.Accounts.Command.Register;
using ShipCapstone.Application.Features.Authentication.Command.Login;
using ShipCapstone.Application.Features.Authentication.Command.SendOtp;
using ShipCapstone.Application.Features.Boatyards.Command.CreateBoatyard;
using ShipCapstone.Application.Features.Boatyards.Command.UpdateBoatyard;
using ShipCapstone.Application.Features.BoatyardServices.Command.CreateBoatyardService;
using ShipCapstone.Application.Features.BoatyardServices.Command.UpdateBoatyardService;
using ShipCapstone.Application.Features.Bookings.Command.CreateBooking;
using ShipCapstone.Application.Features.Captains.Command.RegisterCaptain;
using ShipCapstone.Application.Features.Categories.Command.CreateCategory;
using ShipCapstone.Application.Features.Categories.Command.UpdateCategory;
using ShipCapstone.Application.Features.DockSlots.Command.CreateDockSlot;
using ShipCapstone.Application.Features.DockSlots.Command.UpdateDockSlot;
using ShipCapstone.Application.Features.ModifierGroups.Command.CreateModifierGroup;
using ShipCapstone.Application.Features.ModifierGroups.Command.UpdateModifierGroup;
using ShipCapstone.Application.Features.ModifierOptions.Command.CreateModifierOption;
using ShipCapstone.Application.Features.ModifierOptions.Command.UpdateModifierOption;
using ShipCapstone.Application.Features.Orders.Command.CreateOrder;
using ShipCapstone.Application.Features.Payments.Command.CreatePayment;
using ShipCapstone.Application.Features.Ports.Command.CreatePort;
using ShipCapstone.Application.Features.Ports.Command.UpdatePort;
using ShipCapstone.Application.Features.Products.Command.CreateProduct;
using ShipCapstone.Application.Features.Products.Command.UpdateProduct;
using ShipCapstone.Application.Features.ProductVariants.Command.CreateProductVariant;
using ShipCapstone.Application.Features.ProductVariants.Command.UpdateProductVariant;
using ShipCapstone.Application.Features.Reviews.Command.CreateReview;
using ShipCapstone.Application.Features.Ships.Command.AssignCaptainToShip;
using ShipCapstone.Application.Features.Ships.Command.CreateShip;
using ShipCapstone.Application.Features.Ships.Command.UpdateShip;
using ShipCapstone.Application.Features.Suppliers.Command.CreateSupplier;
using ShipCapstone.Application.Services.Implements;
using ShipCapstone.Application.Services.Interfaces;
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
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IClaimService, ClaimService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IRedisService, RedisService>();
        services.AddScoped<IUploadService, UploadService>();
        services.AddScoped<IOAuthService, OAuthService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped(typeof(ValidationUtil<>));
        services.AddScoped<IValidator<CreateOrderCommand>, CreateOrderCommandValidator>();
        services.AddScoped<IValidator<LoginCommand>, LoginCommandValidator>();
        services.AddScoped<IValidator<SendOtpCommand>, SendOtpCommandValidator>();
        services.AddScoped<IValidator<RegisterCommand>, RegisterCommandValidator>();
        services.AddScoped<IValidator<CreateShipCommand>, CreateShipCommandValidator>();
        services.AddScoped<IValidator<UpdateShipCommand>, UpdateShipCommandValidator>();
        services.AddScoped<IValidator<CreateSupplierCommand>, CreateSupplierCommandValidator>();
        services.AddScoped<IValidator<CreateCategoryCommand>, CreateCategoryCommandValidator>();
        services.AddScoped<IValidator<UpdateCategoryCommand>, UpdateCategoryCommandValidator>();
        services.AddScoped<IValidator<CreateModifierGroupCommand>, CreateModifierGroupCommandValidator>();
        services.AddScoped<IValidator<UpdateModifierGroupCommand>, UpdateModifierGroupCommandValidator>();
        services.AddScoped<IValidator<CreateModifierOptionCommand>, CreateModifierOptionCommandValidator>();
        services.AddScoped<IValidator<UpdateModifierOptionCommand>, UpdateModifierOptionCommandValidator>();
        services.AddScoped<IValidator<CreatePortCommand>, CreatePortCommandValidator>();
        services.AddScoped<IValidator<UpdatePortCommand>, UpdatePortCommandValidator>();
        services.AddScoped<IValidator<CreateBoatyardCommand>, CreateBoatyardCommandValidator>();
        services.AddScoped<IValidator<UpdateBoatyardCommand>, UpdateBoatyardCommandValidator>();
        services.AddScoped<IValidator<CreatePaymentCommand>, CreatePaymentCommandValidator>();
        services.AddScoped<IValidator<CreateDockSlotCommand>, CreateDockSlotCommandValidator>();
        services.AddScoped<IValidator<UpdateDockSlotCommand>, UpdateDockSlotCommandValidator>();
        services.AddScoped<IValidator<CreateBoatyardServiceCommand>, CreateBoatyardServiceCommandValidator>();
        services.AddScoped<IValidator<UpdateBoatyardServiceCommand>, UpdateBoatyardServiceCommandValidator>();
        services.AddScoped<IValidator<AssignCaptainToShipCommand>, AssignCaptainToShipCommandValidator>();
        services.AddScoped<IValidator<RegisterCaptainCommand>, RegisterCaptainCommandValidator>();
        services.AddScoped<IValidator<CreateProductCommand>, CreateProductCommandValidator>();
        services.AddScoped<IValidator<UpdateProductCommand>, UpdateProductCommandValidator>();
        services.AddScoped<IValidator<CreateProductVariantCommand>, CreateProductVariantCommandValidator>();
        services.AddScoped<IValidator<UpdateProductVariantCommand>, UpdateProductVariantCommandValidator>();
        services.AddScoped<IValidator<CreateBookingCommand>, CreateBookingCommandValidator>();
        services.AddScoped<IValidator<CreateReviewCommand>, CreateReviewCommandValidator>();
        services.AddScoped<IValidator<ChangePasswordCommand>, ChangePasswordCommandValidator>();
        services.AddHttpContextAccessor();
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