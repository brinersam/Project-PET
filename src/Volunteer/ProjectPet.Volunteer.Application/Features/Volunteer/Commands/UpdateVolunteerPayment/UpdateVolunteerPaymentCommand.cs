using ProjectPet.Application.Dto;

namespace ProjectPet.VolunteerModule.Application.Features.Volunteer.Commands.UpdateVolunteerPayment;

public record UpdateVolunteerPaymentCommand(
    Guid Id,
    List<PaymentInfoDto> PaymentInfos);
