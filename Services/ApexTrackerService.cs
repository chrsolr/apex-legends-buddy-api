using System.Globalization;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;

public class ApexTrackerService : IApexTrackerService
{
    private readonly HttpClient httpClient;
    private readonly DataContext context;

    public ApexTrackerService(HttpClient _httpClient, DataContext _context)
    {
        httpClient = _httpClient;
        context = _context;
    }

    public async Task<List<UsageRateDTO>> GetLegendUsageRates()
    {
        return await context
            .UsageRates.Select(legend => new UsageRateDTO()
            {
                Name = legend.Name,
                Rate = legend.Rate,
                KPM = legend.KPM
            })
            .OrderBy(legend => legend.Name)
            .ToListAsync();
    }

    public async Task<UsageRateDTO?> GetLegendUsageRateByName(string legendName)
    {
        return await context
            .UsageRates.Where(rate => rate.Name.ToUpper() == legendName.ToUpper())
            .Select(rate => new UsageRateDTO()
            {
                Name = rate.Name,
                Rate = rate.Rate,
                KPM = rate.KPM
            })
            .FirstOrDefaultAsync();
    }

    public async Task UpdateLegendUsageRates()
    {
        var usageRates = await GetUsageRatesFromApexTracker();
        foreach (var rate in usageRates)
        {
            var usageRate = context.UsageRates.FirstOrDefault(v => v.Name == rate.Name);
            if (usageRate is not null)
            {
                usageRate.Name = rate.Name;
                usageRate.Rate = rate.Rate;
            }
            else
            {
                context.UsageRates.Add(
                    new UsageRate()
                    {
                        Id = Guid.NewGuid(),
                        Name = rate.Name,
                        Rate = rate.Rate,
                        KPM = rate.KPM,
                    }
                );
            }

            await context.SaveChangesAsync();
        }
    }

    private async Task<List<UsageRateDTO>> GetUsageRatesFromApexTracker()
    {
        const string url = "https://tracker.gg/apex/insights";
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation(
            "User-Agent",
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36"
        );

        var html = await httpClient.GetStringAsync(url);
        if (html is null)
        {
            throw new Exception("Failed to get usage rates from Service");
        }

        var document = new HtmlDocument();
        document.LoadHtml(html);

        var usageRoot = document.DocumentNode.SelectNodes(
            ".//div[contains(@class, 'usage__content')]//div[contains(@class, 'insight-bar')]"
        );

        var usage = (
            from element in usageRoot

            let name = element
                .SelectSingleNode(".//div[contains(@class, 'insight-bar__label')]")
                ?.GetDirectInnerText()
                .Trim()
            let usageRate = element
                .SelectSingleNode(".//div[contains(@class, 'insight-bar__value')]")
                ?.GetDirectInnerText()
                .Trim()
                .Replace("%", "")

            select new { Name = name, UsageRate = Convert.ToDecimal(usageRate) }
        ).Where(v => v.Name is not null).ToList();

        var kpmRoot = document.DocumentNode.SelectNodes(".//table//tbody//tr");
        var kpms = (
            from element in kpmRoot

            let name = element.SelectSingleNode(".//td[1]")?.GetDirectInnerText().Trim()
            let kpm = element.SelectSingleNode(".//td[2]")?.GetDirectInnerText().Trim()

            select new { Name = name, KPM = kpm ?? "N/A" }
        ).ToList();

        return usage
            .Select(element => new UsageRateDTO()
            {
                Name = element.Name,
                Rate = (element?.UsageRate ?? 0).ToString(CultureInfo.InvariantCulture),
                KPM = kpms.Find(kpm => kpm.Name == element?.Name)!.KPM ?? "N/A"
            })
            .OrderBy(usage => usage.Name)
            .ToList();
    }
}
