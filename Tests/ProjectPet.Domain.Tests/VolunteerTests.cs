using ProjectPet.Domain.Models;

namespace ProjectPet.Domain.Tests;

public class VolunteerTests
{
    [Fact]
    public void Pets_MovePos2TO2_NoChange()
    {
        var volunteer = VolunteerWithPets(3);
        Pet[] pets = volunteer.OwnedPets.ToArray();

        volunteer.SetPetPosition(2, 2);

        Assert.Equal(1, pets[0].OrderingPosition.Value);
        Assert.Equal(2, pets[1].OrderingPosition.Value);
        Assert.Equal(3, pets[2].OrderingPosition.Value);
    }

    [Fact]
    public void Pets_MovePos1TOBack_CorrectOrder()
    {
        var volunteer = VolunteerWithPets(4);
        Pet[] pets = volunteer.OwnedPets.ToArray();

        volunteer.SetPetPositionToBack(1);

        Assert.Equal(4, pets[0].OrderingPosition.Value);
        Assert.Equal(1, pets[1].OrderingPosition.Value);
        Assert.Equal(2, pets[2].OrderingPosition.Value);
        Assert.Equal(3, pets[3].OrderingPosition.Value);
    }

    [Fact]
    public void Pets_MovePos4TOFront_CorrectOrder()
    {
        var volunteer = VolunteerWithPets(4);
        Pet[] pets = volunteer.OwnedPets.ToArray();

        volunteer.SetPetPositionToFront(4);

        Assert.Equal(2, pets[0].OrderingPosition.Value);
        Assert.Equal(3, pets[1].OrderingPosition.Value);
        Assert.Equal(4, pets[2].OrderingPosition.Value);
        Assert.Equal(1, pets[3].OrderingPosition.Value);
    }

    [Fact]
    public void Pets_MovePos4TO1_CorrectOrder()
    {
        var volunteer = VolunteerWithPets(4);
        Pet[] pets = volunteer.OwnedPets.ToArray();

        volunteer.SetPetPosition(4, 1);

        Assert.Equal(2, pets[0].OrderingPosition.Value);
        Assert.Equal(3, pets[1].OrderingPosition.Value);
        Assert.Equal(4, pets[2].OrderingPosition.Value);
        Assert.Equal(1, pets[3].OrderingPosition.Value);
    }


    [Fact]
    public void Pets_MovePos1TO4_CorrectOrder()
    {
        var volunteer = VolunteerWithPets(4);
        Pet[] pets = volunteer.OwnedPets.ToArray();

        volunteer.SetPetPosition(1, 4);

        Assert.Equal(4, pets[0].OrderingPosition.Value);
        Assert.Equal(1, pets[1].OrderingPosition.Value);
        Assert.Equal(2, pets[2].OrderingPosition.Value);
        Assert.Equal(3, pets[3].OrderingPosition.Value);
    }

    private Volunteer VolunteerWithPets(int amntOfPets)
    {
        var volunteer = CreateVolunteer();

        var pets = Enumerable.Range(1, amntOfPets)
            .Select(x => CreatePet(volunteer, x))
            .ToArray();

        foreach (var pet in pets)
            volunteer.AddPet(pet);

        return volunteer;
    }

    private Volunteer CreateVolunteer(int idx = 1)
    {
        return Volunteer.Create(
            Guid.NewGuid(),
            $"{idx}:fullName",
            $"{idx}gmail@email.com",
            $"{idx}:description",
            1,
            Phonenumber.Create("123-456-78-98", $"{idx}").Value,
            [],
            []).Value;
    }

    private Pet CreatePet(Volunteer volunteer, int idx = 1)
    {
        var animalData = AnimalData.Create(Guid.NewGuid(), Guid.NewGuid());
        var phonenumber = Phonenumber.Create($"123-456-90-90", $"{idx}");
        var address = Address.Create("name", "street", "building", null, null, null, null);
        var healthInfo = HealthInfo.Create("healthy", true, true, 1, 1);
        var orderingPosition = Position.Create(idx);
        var pet = Pet.Create(
            "Name",
            animalData.Value,
            "Description",
            "Coat",
            orderingPosition,
            healthInfo.Value,
            address.Value,
            phonenumber.Value,
            Status.Looking_For_Home,
            DateOnly.FromDateTime(DateTime.Now),
            [],
            []);
        return pet.Value;
    }
}