using HtmlAgilityPack;

namespace apex_legends_buddy_api.Services;

public class GamepediaService(HttpClient httpClient, IApexTrackerService apexTrackerService)
    : IGamepediaService
{
    public async Task<List<Legend>?> GetLegends()
    {
        const string url =
            "https://apexlegends.gamepedia.com/api.php?action=parse&format=json&page=Legends&redirects=1";
        var response = await httpClient.GetFromJsonAsync<GamepediaResponse>(url);
        if (response == null)
        {
            return null;
        }

        var insights = await apexTrackerService.GetUsageRates();
        var html = response.Parse.Text.Asterisk;
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);

        var elements = htmlDocument.DocumentNode.SelectNodes(
            ".//ul[contains(@class, 'gallery') and contains(@class, 'mw-gallery-nolines')]//li[contains(@class, 'gallerybox')]");

        return (from element in elements
            let legendImageNode = element.SelectSingleNode(".//div[contains(@class, 'thumb')]//div//a//img")
            let dataSource = legendImageNode?.GetAttributeValue("data-src", "")
            let imageSource = legendImageNode?.GetAttributeValue("src", "")
            let name = element.SelectSingleNode(".//div[contains(@class, 'gallerytext')]//p//a")?.InnerText.Trim()
            let description = element.SelectSingleNode(".//div[contains(@class, 'gallerytext')]//p")
                ?.GetDirectInnerText().Trim()
            let imageUrl =
                Utils.CleanRevisionImageUrl((string.IsNullOrEmpty(dataSource) ? imageSource : dataSource) ??
                                            string.Empty)
            let className = element.ParentNode.PreviousSibling.PreviousSibling.PreviousSibling.PreviousSibling
                .SelectSingleNode(".//span[contains(@class, 'mw-headline')]")?.GetDirectInnerText().Trim()
            let classDescription = element.ParentNode.PreviousSibling.PreviousSibling.InnerText.Trim()
            let classIconUrl = Utils.CleanRevisionImageUrl(element.ParentNode.PreviousSibling.PreviousSibling
                .PreviousSibling.PreviousSibling.SelectSingleNode(".//a//img").GetAttributeValue("data-src", ""))
            select new Legend()
            {
                Name = name,
                Description = description,
                ImageUrl = imageUrl,
                ClassName = className,
                ClassDescription = classDescription,
                ClassIconUrl = classIconUrl,
                UsageRate = insights.Find(insight => insight.Name == name),
            }).OrderBy(element => element.Name).ToList();
    }
}