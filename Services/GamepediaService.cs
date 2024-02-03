using HtmlAgilityPack;

public class GamepediaService : IGamepediaService
{
    private readonly HttpClient httpClient;
    private readonly IApexTrackerService apexTrackerService;

    public GamepediaService(HttpClient _httpClient, IApexTrackerService _apexTrackerService)
    {
        httpClient = _httpClient;
        apexTrackerService = _apexTrackerService;
    }

    public async Task<List<Legend>> GetLegends()
    {
        const string url =
            "https://apexlegends.gamepedia.com/api.php?action=parse&format=json&page=Legends&redirects=1";

        var response = await httpClient.GetFromJsonAsync<GamepediaResponse>(url);
        if (response is null)
        {
            throw new Exception("Failed to get legends from Service");
        }

        var insights = await apexTrackerService.GetUsageRates();
        var html = response.Parse.Text.Asterisk;
        var document = new HtmlDocument();
        document.LoadHtml(html);

        var elements = document.DocumentNode.SelectNodes(
            ".//ul[contains(@class, 'gallery') and contains(@class, 'mw-gallery-nolines')]//li[contains(@class, 'gallerybox')]"
        );

        return (
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

            select new Legend()
            {
                Name = name,
                Description = description,
                ImageUrl = Utils.CleanRevisionImageUrl(imageUrl),
                ClassName = className,
                ClassDescription = classDescription,
                ClassIconUrl = Utils.CleanRevisionImageUrl(classIconUrl),
                UsageRate = insights.Find(insight => insight.Name == name)
            }
        ).OrderBy(legend => legend.Name).ToList();
    }
}
