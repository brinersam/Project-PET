namespace ProjectPet.Domain.Models
{
    public record Address
    {
        public string Name { get; } = null!;
        public string Street { get; } = null!;
        public string Building { get; } = null!;
        public string? Block { get; }
        public int? Entrance { get; }
        public int? Floor { get; }
        public int Apartment { get; }

        protected Address(
            string name,
            string street,
            string building,
            string? block,
            int? entrance,
            int? floor,
            int apartment)
        {
            Name = name;    
            Street = street;
            Building = building;
            Block = block;
            Entrance = entrance;
            Floor = floor;
            Apartment = apartment;
        }

        public static Address Create( // TODO change return to result, replace returns and raises to result
            string name,
            string street,
            string building,
            string? block,
            int? entrance,
            int? floor,
            int apartment)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("Name argument should not be empty"); 
                                                                                        
            if (String.IsNullOrEmpty(street))
                throw new ArgumentNullException("Street argument should not be empty");

            if (String.IsNullOrEmpty(building))
                throw new ArgumentNullException("Building argument should not be empty");

            return new Address
            (
                name,
                street,
                building,
                block,
                entrance,
                floor,
                apartment
            );
        }

    }
}