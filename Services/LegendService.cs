using Microsoft.EntityFrameworkCore;

public class LegendService : ILegendService
{
    private readonly DataContext context;

    public LegendService(DataContext _context)
    {
        context = _context;
    }

    public async Task<List<LegendDTO>> GetLegends()
    {
        return await context
            .Legends.Select(legend => new LegendDTO()
            {
                Name = legend.Name,
                Description = legend.Description,
                ImageUrl = legend.ImageUrl,
                ClassName = legend.Class.Name,
                ClassDescription = legend.Class.Description,
                ClassIconUrl = legend.Class.IconUrl,
                Lore = legend.Lore.Select(lore => lore.Lore).ToList()
            })
            .OrderBy(legend => legend.Name)
            .ToListAsync();
    }

    public async Task<LegendDTO?> GetLegendsByName(string legendName)
    {
        return await context
            .Legends.Where(legend => legend.Name.ToUpper() == legendName.ToUpper())
            .Select(legend => new LegendDTO()
            {
                Name = legend.Name,
                Description = legend.Description,
                ImageUrl = legend.ImageUrl,
                ClassName = legend.Class.Name,
                ClassDescription = legend.Class.Description,
                ClassIconUrl = legend.Class.IconUrl
            })
            .FirstOrDefaultAsync();
    }
}
