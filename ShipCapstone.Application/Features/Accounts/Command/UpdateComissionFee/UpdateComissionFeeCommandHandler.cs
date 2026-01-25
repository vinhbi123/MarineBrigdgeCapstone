using Mediator;
using ShipCapstone.Application.Common.Exceptions;
using ShipCapstone.Domain.Entities;
using ShipCapstone.Domain.Enums;
using ShipCapstone.Domain.Models.Common;
using ShipCapstone.Infrastructure.Persistence;
using ShipCapstone.Infrastructure.Repositories.Interface;

namespace ShipCapstone.Application.Features.Accounts.Command.UpdateComissionFee;

public class UpdateComissionFeeCommandHandler : IRequestHandler<UpdateComissionFeeCommand, ApiResponse>
{
    private readonly IUnitOfWork<ShipCapstoneContext> _unitOfWork;

    public UpdateComissionFeeCommandHandler(IUnitOfWork<ShipCapstoneContext> unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async ValueTask<ApiResponse> Handle(UpdateComissionFeeCommand request, CancellationToken cancellationToken)
    {
        switch (request.Type)
        {
            case ETypeUpdate.Boatyard:
            {
                var boatyard = await _unitOfWork.GetRepository<Boatyard>().SingleOrDefaultAsync(
                    predicate: x => x.Id == request.Id) ?? throw new NotFoundException("Không tìm thấy xưởng");
                boatyard.CommissionFeePercent = request.CommissionFeePercent ?? boatyard.CommissionFeePercent;
                _unitOfWork.GetRepository<Boatyard>().UpdateAsync(boatyard);
                break;
            }
            case ETypeUpdate.Supplier:
            {
                var supplier = await _unitOfWork.GetRepository<Supplier>().SingleOrDefaultAsync(
                    predicate: x => x.Id == request.Id) ?? throw new NotFoundException("Không tìm thấy xưởng");
                supplier.CommissionFeePercent = request.CommissionFeePercent ?? supplier.CommissionFeePercent;
                _unitOfWork.GetRepository<Supplier>().UpdateAsync(supplier);
                break;
            }
            default:
                throw new BadHttpRequestException("Không có loại hợp lệ");
        }
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            throw new Exception("Có lỗi trong quá trình cập nhật");
        }

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Cập nhật thành công",
            Data = request.Id
        };
    }
}