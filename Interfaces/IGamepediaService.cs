public interface IGamepediaService
{
    public Task<List<LegendDTO>> GetLegends(string? legendName);
    public Task UpdateLegends(string? legendName);
}
