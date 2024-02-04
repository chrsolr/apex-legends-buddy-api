using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;

public class GamepediaService : IGamepediaService
{
    private readonly HttpClient httpClient;
    private readonly DataContext context;

    public GamepediaService(HttpClient _httpClient, DataContext _context)
    {
        httpClient = _httpClient;
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
                ClassIconUrl = legend.Class.IconUrl
            })
            .OrderBy(legend => legend.Name)
            .ToListAsync();
    }

    public async Task UpdateLegends(string? legendName)
    {
        var legends = await GetLegendsFromWiki(legendName);
        foreach (var legend in legends)
        {
            if (context.Legends.Any(v => v.Name == legend.Name))
            {
                continue;
            }

            var legendClass = context.LegendClasses.FirstOrDefault(v => v.Name == legend.ClassName);
            context.Legends.Add(
                new Legend()
                {
                    Id = Guid.NewGuid(),
                    Name = legend.Name,
                    Description = legend.Description,
                    ImageUrl = legend.ImageUrl,
                    Class =
                        legendClass
                        ?? new LegendClass()
                        {
                            Id = Guid.NewGuid(),
                            Name = legend.ClassName,
                            Description = legend.ClassDescription,
                            IconUrl = legend.ClassIconUrl,
                        }
                }
            );
            await context.SaveChangesAsync();
        }
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

    private async Task<List<LegendDTO>> GetLegendsFromWiki(string? legendName)
    {
        const string url =
            "https://apexlegends.gamepedia.com/api.php?action=parse&format=json&page=Legends&redirects=1";

        var response = await httpClient.GetFromJsonAsync<GamepediaResponse>(url);
        if (response is null)
        {
            throw new Exception("Failed to get legends from Service");
        }

        var html = response.Parse.Text.Asterisk;
        var document = new HtmlDocument();
        document.LoadHtml(html);

        var elements = document.DocumentNode.SelectNodes(
            ".//ul[contains(@class, 'gallery') and contains(@class, 'mw-gallery-nolines')]//li[contains(@class, 'gallerybox')]"
        );

        var legends = (
            from element in elements
            let imageNode = element.SelectSingleNode(
                ".//div[contains(@class, 'thumb')]//div//a//img"
            )
            let dataSource = imageNode.GetAttributeValue("data-src", "")
            let imageSource = imageNode.GetAttributeValue("src", "")
            let name = element
                .SelectSingleNode(".//div[contains(@class, 'gallerytext')]//p//a")
                .InnerText.Trim()
            let description = element
                .SelectSingleNode(".//div[contains(@class, 'gallerytext')]//p")
                .GetDirectInnerText()
                .Trim()
            let imageUrl = String.IsNullOrEmpty(dataSource)
                ? imageSource
                : dataSource ?? string.Empty
            let className = element
                .ParentNode.PreviousSibling.PreviousSibling.PreviousSibling.PreviousSibling.SelectSingleNode(
                    ".//span[contains(@class, 'mw-headline')]"
                )
                .GetDirectInnerText()
                .Trim()
            let classDescription = element.ParentNode.PreviousSibling.PreviousSibling.InnerText.Trim()
            let classIconUrl = element
                .ParentNode.PreviousSibling.PreviousSibling.PreviousSibling.PreviousSibling.SelectSingleNode(
                    ".//a//img"
                )
                .GetAttributeValue("data-src", "")

            select new LegendDTO()
            {
                Name = name,
                Description = description,
                ImageUrl = Utils.CleanRevisionImageUrl(imageUrl),
                ClassName = className,
                ClassDescription = classDescription,
                ClassIconUrl = Utils.CleanRevisionImageUrl(classIconUrl),
            }
        ).OrderBy(legend => legend.Name);

        return string.IsNullOrEmpty(legendName)
            ? legends.ToList()
            : legends.Where(legend => legend.Name.ToUpper() == legendName.ToUpper()).ToList();
    }
}
