using DockerTestsSample.PopulationDbContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DockerTestsSample.PopulationDbContext;

public interface IPopulationDbContext
{
    DbSet<Person> People { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    DatabaseFacade Database { get; }
}