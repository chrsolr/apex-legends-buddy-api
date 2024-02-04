public interface IGamepediaService
{
    public Task<List<LegendDTO>> GetLegends(string? legendName);
}
