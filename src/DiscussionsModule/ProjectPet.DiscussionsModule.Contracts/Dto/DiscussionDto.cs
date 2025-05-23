namespace ProjectPet.DiscussionsModule.Contracts.Dto;
public class DiscussionDto
{
    public Guid Id { get; init; }
    public Guid RelatedEntityId { get; init; }
    public bool IsClosed { get; init; }
    public IReadOnlyList<Guid> UserIds { get; init; } = [];
    public List<MessageDto> Messages { get; init; } = [];
}
