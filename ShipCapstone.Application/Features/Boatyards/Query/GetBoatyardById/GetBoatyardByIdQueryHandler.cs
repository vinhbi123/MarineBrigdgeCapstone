using Mediator;
using Microsoft.EntityFrameworkCore;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Models.Boatyards;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Boatyards.Query.GetBoatyardById;

public class GetBoatyardByIdQueryHandler : IRequestHandler<GetBoatyardByIdQuery, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;
    private readonly ILogger _logger;
    
    public GetBoatyardByIdQueryHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork, ILogger logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async ValueTask<ApiResponse> Handle(GetBoatyardByIdQuery request, CancellationToken cancellationToken)
    {
        var boatyard = await _unitOfWork.GetRepository<Boatyard>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.BoatyardId,
            include: x => x.Include(x => x.Account)
        );
        
        if(boatyard == null)
            throw new NotFoundException("Xưởng không tồn tại");

        var response = new GetBoatyardByIdResponse()
        {
            Id = boatyard.Id,
            Name = boatyard.Name,
            Latitude = boatyard.Latitude,
            Longitude = boatyard.Longitude,
            AccountId = boatyard.AccountId,
            FullName = boatyard.Account.FullName,
            Username = boatyard.Account.Username,
            Address = boatyard.Account.Address,
            Email = boatyard.Account.Email,
            PhoneNumber = boatyard.Account.PhoneNumber,
            AvatarUrl = boatyard.Account.AvatarUrl,
            CreatedDate = boatyard.CreatedDate,
            LastModifiedDate = boatyard.LastModifiedDate
        };

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Lấy thông tin xưởng thành công",
            Data = response
        };
    }
}