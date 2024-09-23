namespace ProjectPet.Domain.Models
{
    public record AnimalData
    {
        public SpeciesID SpeciesID { get; } = null!;
        public Guid BreedID { get; }
        protected AnimalData(SpeciesID speciesID, Guid breedID)
        {
            SpeciesID = speciesID;
            BreedID = breedID;
        }
        public static AnimalData Create(SpeciesID speciesID, Guid breedID)
        {
            return new AnimalData(speciesID, breedID);
        }

    }
}