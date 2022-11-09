using DockerTestsSample.PopulationDbContext.Entities;
using DockerTestsSample.PopulationDbContext.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace DockerTestsSample.PopulationDbContext;

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