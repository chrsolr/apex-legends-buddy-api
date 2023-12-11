using System.Globalization;
using apex_legends_buddy_api.Interfaces;
using HtmlAgilityPack;

namespace apex_legends_buddy_api.Services;

public class ApexTrackerService(HttpClient httpClient) : IApexTrackerService
{
    public async Task<List<UsageRate>?> GetUsageRates()
    {
        const string url = "https://tracker.gg/apex/insights";
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent",
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");

        var html = await httpClient.GetStringAsync(url);
        if (string.IsNullOrEmpty(html))
        {
            return null;
        }

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);

        var usageRoot = htmlDocument.DocumentNode.SelectNodes(
            ".//div[contains(@class, 'usage__content')]//div[contains(@class, 'insight-bar')]");

        var usage = (from element in usageRoot
            let name = element.SelectSingleNode(".//div[contains(@class, 'insight-bar__label')]")
                ?.GetDirectInnerText().Trim()
            let usageRate = element.SelectSingleNode(".//div[contains(@class, 'insight-bar__value')]")
                ?.GetDirectInnerText().Trim().Replace("%", "")
            select new
            {
                Name = name,
                UsageRate = Convert.ToDecimal(usageRate)
            }).Where(v => v.Name is not null).ToList();

        var kpmRoot = htmlDocument.DocumentNode.SelectNodes(
            ".//table//tbody//tr");

        var kpms = (from element in kpmRoot
                let name = element.SelectSingleNode(".//td[1]").GetDirectInnerText().Trim()
                let kpm = element.SelectSingleNode(".//td[2]").GetDirectInnerText().Trim()
                select new
                {
                    Name = name,
                    Kpm = kpm ?? "N/A"
                }
            ).ToList();

        return usage.Select(element => new UsageRate()
        {
            Name = element.Name,
            Rate = (element?.UsageRate ?? 0).ToString(CultureInfo.InvariantCulture),
            Kpm = kpms.Find(value => value.Name == element?.Name)!.Kpm ?? "N/A"
        }).ToList();
    }
}