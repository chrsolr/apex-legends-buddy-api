public interface IGamepediaService
{
    public Task<List<LegendDTO>> GetLegends();
    public Task<LegendDTO?> GetLegendsByName(string legendName);
    public Task UpdateLegends(string? legendName);
}
