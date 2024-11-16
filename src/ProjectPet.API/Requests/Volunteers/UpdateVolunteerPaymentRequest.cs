using ProjectPet.Application.Dto;

namespace ProjectPet.API.Requests.Volunteers;

public record class UpdateVolunteerPaymentRequest(List<PaymentInfoDto> PaymentInfos);
