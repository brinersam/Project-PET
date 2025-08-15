using System.Runtime.Serialization;

namespace ProjectPet.SharedKernel.Exceptions;
[Serializable]
public class IntegrationEventErroredException : Exception
{
    public IntegrationEventErroredException(string message)
        : base(message)
    { }

    protected IntegrationEventErroredException(SerializationInfo info, StreamingContext ctxt)
        : base(info, ctxt)
    { }
}