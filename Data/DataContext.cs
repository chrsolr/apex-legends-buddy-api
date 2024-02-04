using Microsoft.EntityFrameworkCore;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options) { }

    public DbSet<Legend> Legends => Set<Legend>();
    public DbSet<UsageRate> UsageRates => Set<UsageRate>();
}
