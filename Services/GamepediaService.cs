using HtmlAgilityPack;

public class GamepediaService : IGamepediaService
{
    private readonly HttpClient httpClient;

    public GamepediaService(HttpClient _httpClient)
    {
        httpClient = _httpClient;
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

        var html = response.Parse.Text.Asterisk;
        var document = new HtmlDocument();
        document.LoadHtml(html);

        var elements = document.DocumentNode.SelectNodes(
            ".//ul[contains(@class, 'gallery') and contains(@class, 'mw-gallery-nolines')]//li[contains(@class, 'gallerybox')]"
        );

        return (
            from element in elements

            select new Legend()
            {
                Name = "",
                Description = "",
                ImageUrl = "",
                ClassName = "",
                ClassDescription = "",
                ClassIconUrl = "",
                UsageRate = new UsageRate()
                {
                    Name = "",
                    Rate = "0",
                    KPM = "0",
                },
            }
        ).OrderBy(legend => legend.Name).ToList();
    }
}
