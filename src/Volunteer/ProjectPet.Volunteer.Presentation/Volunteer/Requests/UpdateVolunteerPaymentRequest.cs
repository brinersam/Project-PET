using ProjectPet.SharedKernel.Dto;

namespace ProjectPet.VolunteerModule.Presentation.Volunteer.Requests;

public record class UpdateVolunteerPaymentRequest(List<PaymentInfoDto> PaymentInfos);
