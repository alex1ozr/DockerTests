using DockerTestsSample.Common.Exceptions;
using DockerTestsSample.Store;
using DockerTestsSample.Store.Entities;
using Microsoft.EntityFrameworkCore;

namespace DockerTestsSample.Repositories.People;

internal sealed class PersonRepository : IPersonRepository
{
    private readonly IPopulationDbContext _dbContext;

    public PersonRepository(IPopulationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<Person>> GetAllAsync(CancellationToken ct)
        => await _dbContext.People
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<Person?> GetAsync(Guid id, CancellationToken ct)
        => await _dbContext.People
            .AsNoTracking()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(ct);

    public async Task CreateAsync(Person person, CancellationToken ct)
    {
        _dbContext.People.Add(person);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Person person, CancellationToken ct)
    {
        _dbContext.People.Update(person);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var entity = await _dbContext.People
                         .FirstOrDefaultAsync(x => x.Id == id, ct)
                     ?? throw new PersonNotFoundException(id);

        _dbContext.People.Remove(entity);
        await _dbContext.SaveChangesAsync(ct);
    }
}
