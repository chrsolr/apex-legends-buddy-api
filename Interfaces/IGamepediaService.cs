public interface IGamepediaService
{
    public Task<List<Legend>> GetLegends(string? legendName);
}
