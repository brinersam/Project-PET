namespace ProjectPet.DiscussionsModule.Contracts.Dto;
public class MessageDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public DateTime CreatedAt { get; init; }
    public string Text { get; init; } = string.Empty;
    public bool IsEdited { get; init; }
}
