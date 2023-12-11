namespace apex_legends_buddy_api.Interfaces;

public interface IGamepediaService
{
    public Task<List<Legend>?> GetLegends();
}