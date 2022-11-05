using DockerTestsSample.PopulationDbContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace DockerTestsSample.PopulationDbContext;

internal sealed class PopulationDbContext : DbContext, IPopulationDbContext
{
    public PopulationDbContext(DbContextOptions opts) : base(opts)
    {
    }

    public DbSet<Person> People => Set<Person>();
}