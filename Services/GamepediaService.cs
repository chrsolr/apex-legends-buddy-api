using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;

record ColorRarity(
    string Heirloom = "#ff4e1d",
    string Legendary = "#cead21",
    string Epic = "#b237c8",
    string Rare = "#51a8d6",
    string Common = "#a8a8a8",
    string Base = "#010101"
);

public class GamepediaService : IGamepediaService
{
    private readonly HttpClient httpClient;
    private readonly DataContext context;
    private readonly ILegendService legendService;

    public GamepediaService(
        HttpClient _httpClient,
        DataContext _context,
        ILegendService _legendService
    )
    {
        httpClient = _httpClient;
        context = _context;
        legendService = _legendService;
    }

    private async Task<int?> GetSectionIndex(string legendName, string sectionName)
    {
        string url =
            $"https://apexlegends.gamepedia.com/api.php?action=parse&format=json&prop=sections&page={legendName}";

        var response = await httpClient.GetFromJsonAsync<GamepediaResponse>(url);
        if (response is null)
        {
            throw new Exception("Failed to get legends from service");
        }

        Console.WriteLine(response.Parse);

        var sectionIndex = response.Parse.Sections.Find(v => v.Anchor == sectionName)?.Index;
        if (String.IsNullOrEmpty(sectionIndex))
        {
            return null;
        }

        return Int32.TryParse(sectionIndex, out int parsedIndex) ? parsedIndex : null;
    }

    public async Task UpdateLegends(string? legendName)
    {
        var legends = await GetLegendsFromWiki(legendName);

        foreach (var legend in legends)
        {
            var dbLegend = await context.Legends.FirstOrDefaultAsync(v => v.Name == legend.Name);
            if (dbLegend is not null)
            {
                context.LegendLores.RemoveRange(dbLegend.Lore);
                context.Legends.Remove(dbLegend);
                await context.SaveChangesAsync();
            }

            var legendClass = context.LegendClasses.FirstOrDefault(v => v.Name == legend.ClassName);
            var lore = await GetLore(legend.Name);
            var skins = await GetSkins(legend.Name);

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
                        },
                    // Lore = lore.Select(lore => new LegendLore { Id = Guid.NewGuid(), Lore = lore, })
                    //     .ToList()
                }
            );
            await context.SaveChangesAsync();
        }
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

    private async Task<List<string>> GetLore(string legendName)
    {
        var sectionIndex = await GetSectionIndex(legendName, "Lore");
        if (sectionIndex is null)
        {
            throw new Exception("Failed to get legends from service");
        }

        string url =
            $"https://apexlegends.gamepedia.com/api.php?action=parse&format=json&prop=text&page={legendName}&section={sectionIndex}";

        var response = await httpClient.GetFromJsonAsync<GamepediaResponse>(url);
        if (response is null)
        {
            throw new Exception("Failed to get legends from service");
        }

        var html = response.Parse.Text.Asterisk;
        var document = new HtmlDocument();
        document.LoadHtml(html);

        var quotes = document
            .DocumentNode.SelectNodes(".//p")
            .Select(v => v.GetDirectInnerText().Trim())
            .ToList<string>();

        return quotes;
    }

    private async Task<List<dynamic>> GetSkins(string legendName)
    {
        var sectionIndex = await GetSectionIndex(legendName, "Skins");
        if (sectionIndex is null)
        {
            throw new Exception("Failed to get legends from service");
        }

        string url =
            $"https://apexlegends.gamepedia.com/api.php?action=parse&format=json&prop=text&page={legendName}&section={sectionIndex}";

        var response = await httpClient.GetFromJsonAsync<GamepediaResponse>(url);
        if (response is null)
        {
            throw new Exception("Failed to get legends from service");
        }

        //     const $ = cheerio.load((await axios.get(url)).data.parse.text['*'])
        //     const $root = $('.tabbertab')
        //
        //     return $root
        //       .map((index, element) => {
        //         const $element = $(element)
        //         const [rarity] = $element.attr('title')?.trim().split(' ') || ['Base']
        //         // @ts-ignore
        //         const color = colors.rarities[rarity]
        //
        //         const skins = $element
        //           .find('.gallerybox')
        //           .map((index, element) => {
        //             const $title = $(element).find('.gallerytext span:eq(0)')
        //             const $skinImage = $(element).find('.thumb img')
        //             const $cost = $(element).find('.gallerytext span:eq(1)')
        //             const costContent = $cost
        //               .contents()
        //               .map((i, v) => $(v).text().trim())
        //               .get()
        //               .filter((v) => v)
        //
        //             const name = $title.text().trim()
        //             const rarity = $title.css('color').trim()
        //             const imageUrl = $skinImage.attr('src')
        //             const materialImageUrl = $cost.find('a img').attr('src')
        //
        //             const [materialCost] = costContent
        //             const requirement = costContent.filter(
        //               (v) => v && v.indexOf('[note') === -1,
        //             )
        //             return {
        //               id: index,
        //               name,
        //               rarity,
        //               imageUrl,
        //               materialImageUrl,
        //               materialCost,
        //               requirement:
        //                 requirement.length && requirement.length > 2
        //                   ? requirement.splice(1, 3).join(' ')
        //                   : undefined,
        //             }
        //           })
        //           .get()
        //         return { rarity, color, skins }
        //       })
        //       .get()
        //


        var html = response.Parse.Text.Asterisk;
        var document = new HtmlDocument();
        document.LoadHtml(html);

        ColorRarity colors = new();

        // ul[contains(@class, 'gallery')
        var root = document.DocumentNode.SelectSingleNode(".//div[contains(@class, 'tabber')]");

        var skins = root.SelectNodes(".//ul[contains(@class, 'gallery mw-gallery-nolines')]")
            .Select(v => new
            {
                Name = v.SelectSingleNode(".//div[contains(@class, 'gallerytext')]//span[1]")
                    .InnerText.Trim(),
            })
            .ToList<dynamic>();

        foreach (var skin in skins)
        {
            Console.WriteLine($"Skins***************************: {skin.Name}");
        }

        return skins;
    }
}
