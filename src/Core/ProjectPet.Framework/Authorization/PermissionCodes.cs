namespace ProjectPet.Framework.Authorization;

/// <summary>
/// Permission codes used in this applications.
/// </summary>
public static class PermissionCodes
{
    public const string AdminMasterkey = "admin.masterkey";
    public const string VolunteerCreate = "volunteer.create";
    public const string VolunteerRead = "volunteer.read";
    public const string VolunteerUpdate = "volunteer.update";
    public const string VolunteerDelete = "volunteer.delete";
    public const string SelfVolunteerUpdate = "self.volunteer.update";
    public const string SelfVolunteerRead = "self.volunteer.read";
    public const string SelfVolunteerDelete = "self.volunteer.delete";
    public const string OwnedPetsCreate = "owned.pets.create";
    public const string OwnedPetsRead = "owned.pets.read";
    public const string OwnedPetsUpdate = "owned.pets.update";
    public const string OwnedPetsDelete = "owned.pets.delete";
    public const string PetsCreate = "pets.create";
    public const string PetsRead = "pets.read";
    public const string PetsUpdate = "pets.update";
    public const string PetsDelete = "pets.delete";
    public const string SpeciesCreate = "species.create";
    public const string SpeciesRead = "species.read";
    public const string SpeciesUpdate = "species.update";
    public const string SpeciesDelete = "species.delete";
}