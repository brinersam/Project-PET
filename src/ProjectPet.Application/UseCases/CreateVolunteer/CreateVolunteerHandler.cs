using ProjectPet.Domain.Models;

namespace ProjectPet.Application.UseCases.CreateVolunteer
{
    public class CreateVolunteerHandler
    {
        private readonly IVolunteerRepository _volunteerRepository;

        public CreateVolunteerHandler(IVolunteerRepository volunteerRepository)
        {
            _volunteerRepository = volunteerRepository;
        }

        public async Task<Guid> HandleAsync( 
            CreateVolunteerRequest request, CancellationToken cancellationToken = default)
        {

            // validation goes here too

            PhoneNumber phoneNumber; //refactor to use result later TODO
            try 
            {
                phoneNumber = PhoneNumber.Create(request.Phonenumber, request.PhonenumberAreaCode);
            }
            catch (Exception ex)
            {
                throw new Exception("Error when creating number!", ex);
            }


            Volunteer result;
            try //refactor to use result later TODO
            {
                result = Volunteer.Create(
                Guid.NewGuid(),
                request.FullName,
                request.Description,
                request.Email,
                request.YOExperience,
                phoneNumber,
                request.OwnedPets,
                request.PaymentMethods,
                request.SocialNetworks);
            }
            catch (Exception ex)
            {
                throw new Exception("Error when creating volunteer!", ex);
            }


            await _volunteerRepository.AddAsync(result, cancellationToken);

            return result.Id; 
        }
    }
}
