namespace ProjectPet.Domain.Models
{
    public record PetPhoto
    {
        public string StoragePath { get; } = null!;
        public bool IsPrimary { get; }

        protected PetPhoto(string storagePath, bool isPrimary)
        {
            StoragePath = storagePath;
            IsPrimary = isPrimary;
        }

        public static PetPhoto Create(string storagePath, bool isPrimary = false)
        {
            if (String.IsNullOrEmpty(storagePath))
                throw new ArgumentNullException("Health argument should not be empty");

            return new PetPhoto(storagePath, isPrimary);
        }
    }
}
