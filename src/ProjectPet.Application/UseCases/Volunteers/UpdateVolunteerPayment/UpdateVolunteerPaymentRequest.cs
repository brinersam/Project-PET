namespace ProjectPet.Application.UseCases.Volunteers.UpdateVolunteerPayment;

public record UpdateVolunteerPaymentRequest(
    Guid Id,
    UpdateVolunteerPaymentRequestDto PaymentInfos)
{ }
