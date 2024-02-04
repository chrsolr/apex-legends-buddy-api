public interface IApexTrackerService
{
    public Task<List<UsageRateDTO>> GetUsageRates(string? legendName);
}
