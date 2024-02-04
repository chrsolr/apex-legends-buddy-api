public interface IApexTrackerService
{
    public Task<List<UsageRateDTO>> GetLegendUsageRates();
    public Task<UsageRateDTO?> GetLegendUsageRateByName(string legendName);
    public Task UpdateLegendUsageRates();
}
