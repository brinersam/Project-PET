using ProjectPet.VolunteerModule.Contracts.Dto;

namespace ProjectPet.VolunteerModule.Contracts.Requests;

public record class UpdateVolunteerPaymentRequest(List<PaymentInfoDto> PaymentInfos);
