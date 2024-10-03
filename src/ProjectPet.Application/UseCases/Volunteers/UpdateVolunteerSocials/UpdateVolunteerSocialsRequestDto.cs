using ProjectPet.Application.UseCases.Volunteers;

namespace ProjectPet.Application.UseCases.Volunteers
{
    public record class UpdateVolunteerSocialsRequestDto(
        List<PaymentInfoDto> PaymentInfos)
    { }
}
