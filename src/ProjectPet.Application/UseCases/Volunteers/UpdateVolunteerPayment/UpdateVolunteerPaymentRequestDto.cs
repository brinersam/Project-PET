namespace ProjectPet.Application.UseCases.Volunteers;

public record class UpdateVolunteerPaymentRequestDto(
    List<PaymentInfoDto> PaymentInfos)
{ }
