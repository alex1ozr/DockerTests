using DockerTestsSample.PopulationDbContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DockerTestsSample.PopulationDbContext;

/// <summary>
/// DbContext of people
/// </summary>
public interface IPopulationDbContext
{
    /// <summary>
    /// All the people
    /// </summary>
    DbSet<Person> People { get; }
    
    /// <summary>
    /// Saves all changes made in this context to the database.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Provides access to database related information and operations for this context.
    /// </summary>
    DatabaseFacade Database { get; }
}