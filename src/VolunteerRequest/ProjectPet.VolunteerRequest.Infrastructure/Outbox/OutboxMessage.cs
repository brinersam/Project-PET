﻿namespace ProjectPet.VolunteerRequests.Infrastructure.Outbox;
public class OutboxMessage
{
    public Guid Id { get; set; }

    public required string Type { get; set; } = string.Empty;

    public required string Payload { get; set; } = string.Empty;

    public required DateTime OccuredOnUtc { get; set; }

    public DateTime? ProcessedOnUtc { get; set; }

    public string? Error { get; set; }
}
