public record Legend
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string ImageUrl { get; init; }
    public required string ClassIconUrl { get; init; }
    public required string ClassName { get; init; }
    public required string ClassDescription { get; init; }
}
