namespace ProjectPet.Application.UseCases.Volunteers;

public record UpdateVolunteerPaymentRequest(
    Guid Id,
    UpdateVolunteerPaymentRequestDto PaymentInfos)
{ }
