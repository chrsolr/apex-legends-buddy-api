public record LegendClass
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string IconUrl { get; init; }
    public required string Description { get; init; }
}
