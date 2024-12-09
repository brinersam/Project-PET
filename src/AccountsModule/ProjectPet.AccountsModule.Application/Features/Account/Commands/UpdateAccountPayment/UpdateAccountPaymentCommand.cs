using ProjectPet.AccountsModule.Contracts.Requests;
using ProjectPet.Core.Abstractions;
using ProjectPet.SharedKernel.SharedDto;

namespace ProjectPet.AccountsModule.Application.Features.Account.Commands.UpdateAccountPayment;

public record UpdateAccountPaymentCommand(List<PaymentInfoDto> PaymentInfos, Guid UserId)
    : IMapFromRequest<UpdateAccountPaymentCommand, UpdateAccountPaymentRequest, Guid>
{
    public static UpdateAccountPaymentCommand FromRequest(UpdateAccountPaymentRequest req, Guid userId)
        => new UpdateAccountPaymentCommand(req.PaymentInfos, userId);
}
