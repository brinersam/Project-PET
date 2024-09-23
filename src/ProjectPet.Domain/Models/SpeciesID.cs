namespace ProjectPet.Domain.Models
{
    public record SpeciesID
    {
        public Guid Value { get; }

        private SpeciesID(Guid value)
        {
            Value = value;
        }

        public SpeciesID New(Guid value) => new SpeciesID(value);
        public SpeciesID New() => new SpeciesID(Guid.NewGuid());
        public SpeciesID Empty() => new SpeciesID(Guid.Empty);
    }
}