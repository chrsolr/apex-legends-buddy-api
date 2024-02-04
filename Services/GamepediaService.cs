using HtmlAgilityPack;

public class GamepediaService : IGamepediaService
{
    private readonly HttpClient httpClient;

    public GamepediaService(HttpClient _httpClient)
    {
        httpClient = _httpClient;
    }

    public async Task<List<LegendDTO>> GetLegends(string? legendName)
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
