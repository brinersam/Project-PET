using ProjectPet.Core.Abstractions;
using ProjectPet.VolunteerModule.Contracts.Dto;
using ProjectPet.VolunteerModule.Contracts.Requests;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteers.Commands.UpdateVolunteerPayment;

public record UpdateVolunteerPaymentCommand(
    Guid Id,
    List<PaymentInfoDto> PaymentInfos) : IMapFromRequest<UpdateVolunteerPaymentCommand, UpdateVolunteerPaymentRequest, Guid>
{
    public static UpdateVolunteerPaymentCommand FromRequest(UpdateVolunteerPaymentRequest req, Guid id)
    {
        return new UpdateVolunteerPaymentCommand(id, req.PaymentInfos);
    }
}
