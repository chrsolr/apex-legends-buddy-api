public interface IApexTrackerService
{
    public Task<List<UsageRate>> GetUsageRates(string? legendName);
}
