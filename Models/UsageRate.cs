public record UsageRate
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Rate { get; init; }
    public required string KPM { get; init; }
}
