using HtmlAgilityPack;

namespace apex_legends_buddy_api.Services;

public class GamepediaService
{
    private readonly HttpClient _httpClient;

    public GamepediaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://apexlegends.gamepedia.com");
    }

    public async Task<object?> GetLegends()
    {
        const string url = "api.php?action=parse&format=json&page=Legends&redirects=1";

        var response = await _httpClient.GetFromJsonAsync<GamepediaResponse>(url);

        if (response == null)
        {
            return null;
        }

        string? html = response.Parse.Text.Asterisk;

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);

        var elements = htmlDocument.DocumentNode.SelectNodes(
            ".//ul[contains(@class, 'gallery') and contains(@class, 'mw-gallery-nolines')]//li[contains(@class, 'gallerybox')]");

        var list = new List<dynamic>();

        foreach (var element in elements)
        {
            var legendImageNode = element.SelectSingleNode(".//div[contains(@class, 'thumb')]//div//a//img");
            var dataSource = legendImageNode?.GetAttributeValue("data-src", "");
            var imageSource = legendImageNode?.GetAttributeValue("src", "");

            var name = element.SelectSingleNode(".//div[contains(@class, 'gallerytext')]//p//a")?.InnerText.Trim();
            var description = element.SelectSingleNode(".//div[contains(@class, 'gallerytext')]//p")?
                .GetDirectInnerText().Trim();

            var imageUrl = CleanImageUrl((string.IsNullOrEmpty(dataSource) ? imageSource : dataSource) ?? string.Empty);

            var className = element.ParentNode.PreviousSibling.PreviousSibling.PreviousSibling.PreviousSibling
                .SelectSingleNode(".//span[contains(@class, 'mw-headline')]")?
                .GetDirectInnerText().Trim();

            var classDescription = element.ParentNode.PreviousSibling.PreviousSibling.InnerText.Trim();

            var classIconUrl = CleanImageUrl(element.ParentNode.PreviousSibling.PreviousSibling.PreviousSibling
                .PreviousSibling
                .SelectSingleNode(".//a//img").GetAttributeValue("data-src", ""));

            list.Add(new { name, description, imageUrl, classIconUrl, className, classDescription });
        }


        return list;
    }

    private string CleanImageUrl(string url)
    {
        return !string.IsNullOrEmpty(url) ? url.Substring(0, url.IndexOf("/revision/", StringComparison.Ordinal)) : url;
    }
}