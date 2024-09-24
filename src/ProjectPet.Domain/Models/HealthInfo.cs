namespace ProjectPet.Domain.Models
{
    public record HealthInfo
    {
        public string Health { get; } = null!;
        public bool IsSterilized { get; }
        public bool IsVaccinated { get; }
        public float Weight { get; }
        public float Height { get; }

        protected HealthInfo(
            string health,
            bool isSterilized,
            bool isVaccinated,
            float weight,
            float height)
        {
            Health = health;
            IsSterilized = isSterilized;
            IsVaccinated = isVaccinated;
            Weight = weight;
            Height = height;
        }

        public static HealthInfo Create(
            string health, // TODO use results
            bool isSterilized,
            bool isVaccinated,
            float weight,
            float height)
        {
            if (String.IsNullOrEmpty(health))
                throw new ArgumentNullException("Health argument should not be empty"); 

            return new HealthInfo
            (
                health,
                isSterilized,
                isVaccinated,
                weight,
                height
            );
        }
    }
}