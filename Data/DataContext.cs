using Microsoft.EntityFrameworkCore;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options) { }

    public DbSet<Legend> Legends => Set<Legend>();
    public DbSet<UsageRate> UsageRates => Set<UsageRate>();
    public DbSet<LegendClass> LegendClasses => Set<LegendClass>();
    public DbSet<LegendLore> LegendLores => Set<LegendLore>();

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder
    //         .Entity<Legend>()
    //         .HasMany(p => p.Lore)
    //         .WithOne(c => c.Legend)
    //         .OnDelete(DeleteBehavior.Cascade);
    // }
}
