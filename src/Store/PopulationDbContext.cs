using DockerTestsSample.Store.Entities;
using DockerTestsSample.Store.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace DockerTestsSample.Store;

internal sealed class PopulationDbContext : DbContext, IPopulationDbContext
{
    public PopulationDbContext(DbContextOptions opts) : base(opts)
    {
    }

    public DbSet<Person> People => Set<Person>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PersonConfiguration).Assembly);
    }
}