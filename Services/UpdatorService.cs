public class UpdatorService : IUpdatorService
{
    private readonly HttpClient httpClient;
    private readonly DataContext context;

    public UpdatorService(HttpClient _httpClient, DataContext _context)
    {
        httpClient = _httpClient;
        context = _context;
    }

    public Task Update()
    {
        // TODO:
        // 1. Get and Add all legends
        // 2. For each legend, get and add:
        //   - info
        //   - Qoute
        //   - Bio
        //   - Abilities
        //   - Skins
        //   - Heirloom
        //   - Finishers
        //   - Loading Screens
        //   - Skydive Emotes
        //   - Videos
        throw new NotImplementedException();
    }
}
