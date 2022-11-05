namespace DockerTestsSample.PopulationDbContext.Entities;

public class Person
{
    public Person(Guid id, string name, string lastName, DateTime birthDate)
    {
        Id = id;
        Name = name;
        LastName = lastName;
        BirthDate = birthDate;
    }

    public Guid Id { get; set; }
    
    public string Name { get; set; }

    public string LastName { get; set; }

    public DateTime BirthDate { get; set; }
    
    public string? Email { get; set; }
}