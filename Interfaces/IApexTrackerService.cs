namespace apex_legends_buddy_api.Interfaces;

public interface IApexTrackerService
{
    public Task<List<UsageRate>?> GetUsageRates();
}