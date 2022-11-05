using DockerTestsSample.PopulationDbContext;
using DockerTestsSample.PopulationDbContext.Entities;
using DockerTestsSample.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace DockerTestsSample.Repositories.Concrete;

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
                     ?? throw new Exception($"{nameof(Person)} with {nameof(Person.Id)}={id} not found");

        _dbContext.People.Remove(entity);
    }
}
