using ProjectPet.Application.Dto;

namespace ProjectPet.Application.UseCases.Volunteers.UpdateVolunteerPayment;

public record class UpdateVolunteerPaymentRequestDto(
    List<PaymentInfoDto> PaymentInfos)
{ }
