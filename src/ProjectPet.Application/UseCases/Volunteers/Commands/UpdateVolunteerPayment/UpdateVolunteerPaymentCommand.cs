using ProjectPet.Application.Dto;

namespace ProjectPet.Application.UseCases.Volunteers.Commands.UpdateVolunteerPayment;

public record UpdateVolunteerPaymentCommand(
    Guid Id,
    List<PaymentInfoDto> PaymentInfos);
