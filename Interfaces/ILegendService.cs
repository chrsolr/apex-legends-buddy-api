public interface ILegendService
{
    public Task<List<LegendDTO>> GetLegends();
    public Task<LegendDTO?> GetLegendsByName(string legendName);
}
