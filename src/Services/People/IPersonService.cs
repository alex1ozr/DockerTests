namespace DockerTestsSample.Services.People;

public interface IPersonService: IBusinessService
{
    Task CreateAsync(PersonDto person, CancellationToken ct = default);

    Task<PersonDto?> GetAsync(Guid id, CancellationToken ct = default);

    Task<IReadOnlyCollection<PersonDto>> GetAllAsync(CancellationToken ct = default);

    Task UpdateAsync(PersonDto person, CancellationToken ct = default);

    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
