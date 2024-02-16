public class Legend
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string ImageUrl { get; set; }
    public required LegendClass Class { get; set; }
    public List<LegendLore>? Lore { get; set; }
    public UsageRate? UsageRate { get; set; }
}
