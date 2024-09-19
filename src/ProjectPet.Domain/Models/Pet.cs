namespace ProjectPet.Domain.Models
{
    public class Pet
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string Species { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public string Breed { get; private set; } = null!;
        public string Coat { get; private set; } = null!;
        public string Health { get; private set; } = null!;
        public string Address { get; private set; } = null!;
        public string WeightKg { get; private set; } = null!;
        public string Height { get; private set; } = null!;
        public string PhoneNumber { get; private set; } = null!;
        public bool Sterilized { get; private set; }
        public bool Vaccinated { get; private set; }
        public Status Status { get; private set; }
        public DateOnly DateOfBirth { get; private set; }
        public DateOnly CreatedOn { get; private set; }

        private List<PaymentInfo> _paymentMethods = [];
        public IReadOnlyList<PaymentInfo> PaymentMethods => _paymentMethods;

        public void AddPaymentMethod(PaymentInfo paymentInfo)
        {
            _paymentMethods.Add(paymentInfo);
        }
    }

    public class PaymentInfo
    {
        public string Title { get; private set; } = null!;
        public string Description { get; private set; } = null!;
    }

    public enum Status
    {
        NotSet,
        Requires_Care,
        Looking_For_Home,
        Home_Found
    }
}

