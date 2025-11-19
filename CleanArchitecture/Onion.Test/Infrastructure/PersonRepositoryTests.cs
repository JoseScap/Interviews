using Microsoft.EntityFrameworkCore;
using Onion.Application.Interfaces;
using Onion.Domain.Entities;
using Onion.Infrastructure.Database;
using Onion.Infrastructure.Repositories;

namespace Onion.Test.Infrastructure;

public class PersonRepositoryTests
{
    private readonly OnionContext _context;
    private readonly IPersonRepository _personRepository;

    public PersonRepositoryTests()
    {
        var databaseName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<OnionContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;

        _context = new OnionContext(options);
        _personRepository = new PersonRepository(_context);
    }

    [Fact]
    public async Task AddAsync_Should_ReturnPerson_When_PersonIsAdded()
    {
        var person = new Person("Joe", "Doe");

        var returnedPerson = await _personRepository.AddAsync(person);

        Assert.Single(_context.People);
        Assert.Equal(person.FirstName, returnedPerson.FirstName);
        Assert.Equal(person.LastName, returnedPerson.LastName);
    }

    [Fact]
    public async Task AddAsync_Should_SavePersonToDatabase_When_PersonIsAdded()
    {
        var person = new Person("Joe", "Doe");

        await _personRepository.AddAsync(person);

        var inMemoryPerson = await _context.People.SingleAsync();

        Assert.Single(_context.People);
        Assert.Equal(person.FirstName, inMemoryPerson.FirstName);
        Assert.Equal(person.LastName, inMemoryPerson.LastName);
    }

    [Fact]
    public async Task AddAsync_Should_AssignId_When_PersonIsAdded()
    {
        var person1 = new Person("Joe", "Doe");
        var person2 = new Person("Jane", "Doe");

        var returnedPerson1 = await _personRepository.AddAsync(person1);
        var returnedPerson2 = await _personRepository.AddAsync(person2);
        var peopleQuantity = await _context.People.CountAsync();

        Assert.Equal(2, peopleQuantity);

        Assert.NotEqual(returnedPerson1.Id, returnedPerson2.Id);

        Assert.NotEqual(0, returnedPerson1.Id);
        Assert.NotEqual(0, returnedPerson2.Id);
    }

    [Fact]
    public async Task GetAllAsync_Should_ReturnEmptyList_When_DatabaseIsEmpty()
    {
        var emptyList = await _personRepository.GetAllAsync();

        Assert.Empty(emptyList);
        Assert.Empty(_context.People);
    }

    [Fact]
    public async Task GetAllAsync_Should_ReturnAllPeople_When_PeopleExist()
    {
        int peopleQuantity = 10;
        var peopleList = new List<Person>();

        for (int i = 0; i < peopleQuantity; i++)
        {
            var person = new Person("Joe " +  i, "Doe " + i);
            peopleList.Add(person);
            await _personRepository.AddAsync(person);
        }

        var savedPeopleList = await _personRepository.GetAllAsync();
        savedPeopleList = savedPeopleList.OrderBy(x => x.Id).ToList();

        for (int i = 0; i < peopleQuantity; i++)
        {
            Assert.Equal(i + 1, savedPeopleList[i].Id);
            Assert.Equal(peopleList[i].FirstName, savedPeopleList[i].FirstName);
            Assert.Equal(peopleList[i].LastName, savedPeopleList[i].LastName);
        }
    }

    [Fact]
    public async Task GetAllAsync_Should_ReturnCorrectCount_When_MultiplePeopleExist()
    {
        int peopleQuantity = 10;

        for (int i = 1; i <= peopleQuantity; i++)
        {
            var person = new Person("Joe " + 1, "Doe " + 1);
            await _personRepository.AddAsync(person);
        }

        var currentQuantity = await _context.People.CountAsync();

        Assert.Equal(peopleQuantity, currentQuantity);
    }

    [Fact]
    public async Task GetByIdAsync_Should_ReturnPerson_When_PersonExists()
    {
        var person = new Person("Joe", "Doe");
        var addedPerson = await _personRepository.AddAsync(person);

        var retrievedPerson = await _personRepository.GetByIdAsync(addedPerson.Id);

        Assert.NotNull(retrievedPerson);
        Assert.Equal(addedPerson.Id, retrievedPerson.Id);
        Assert.Equal(person.FirstName, retrievedPerson.FirstName);
        Assert.Equal(person.LastName, retrievedPerson.LastName);
    }

    [Fact]
    public async Task GetByIdAsync_Should_ReturnNull_When_PersonDoesNotExist()
    {
        var nonExistentId = 999;

        var retrievedPerson = await _personRepository.GetByIdAsync(nonExistentId);

        Assert.Null(retrievedPerson);
    }

    [Fact]
    public async Task GetByIdAsync_Should_ReturnCorrectPerson_When_MultiplePeopleExist()
    {
        var person1 = new Person("Joe", "Doe");
        var person2 = new Person("Jane", "Smith");
        var person3 = new Person("Bob", "Johnson");

        var addedPerson1 = await _personRepository.AddAsync(person1);
        var addedPerson2 = await _personRepository.AddAsync(person2);
        var addedPerson3 = await _personRepository.AddAsync(person3);

        var retrievedPerson = await _personRepository.GetByIdAsync(addedPerson2.Id);

        Assert.NotNull(retrievedPerson);
        Assert.Equal(addedPerson2.Id, retrievedPerson.Id);
        Assert.Equal(person2.FirstName, retrievedPerson.FirstName);
        Assert.Equal(person2.LastName, retrievedPerson.LastName);
    }

    [Fact]
    public async Task SaveChangesAsync_Should_PersistChanges_When_Called()
    {
        // Arrange: Add multiple people to context without saving
        var person1 = new Person("Joe", "Doe");
        var person2 = new Person("Jane", "Smith");
        var person3 = new Person("Bob", "Johnson");

        _context.People.Add(person1);
        _context.People.Add(person2);
        _context.People.Add(person3);

        // Verify changes are not yet persisted
        var countBeforeSave = await _context.People.CountAsync();
        Assert.Equal(0, countBeforeSave);

        // Act: Save changes through repository
        await _personRepository.SaveChangesAsync();

        // Assert: Verify all changes were persisted
        var countAfterSave = await _context.People.CountAsync();
        Assert.Equal(3, countAfterSave);

        var savedPeople = await _context.People.ToListAsync();
        Assert.Contains(savedPeople, p => p.FirstName == person1.FirstName && p.LastName == person1.LastName);
        Assert.Contains(savedPeople, p => p.FirstName == person2.FirstName && p.LastName == person2.LastName);
        Assert.Contains(savedPeople, p => p.FirstName == person3.FirstName && p.LastName == person3.LastName);
    }

    [Fact]
    public async Task SaveChangesAsync_Should_PersistUpdates_When_EntitiesAreModified()
    {
        // Arrange: Add and save initial person
        var person = new Person("Joe", "Doe");
        _context.People.Add(person);
        await _context.SaveChangesAsync();

        // Modify entity in context
        person.FirstName = "John";
        person.LastName = "Updated";
        _context.People.Update(person);

        // Act: Save changes through repository
        await _personRepository.SaveChangesAsync();

        // Assert: Verify updates were persisted
        var updatedPerson = await _context.People.FirstAsync(p => p.Id == person.Id);
        Assert.Equal("John", updatedPerson.FirstName);
        Assert.Equal("Updated", updatedPerson.LastName);
    }

    [Fact]
    public async Task SaveChangesAsync_Should_PersistMultipleChanges_When_AddingAndUpdating()
    {
        // Arrange: Add initial person and save
        var existingPerson = new Person("Joe", "Doe");
        _context.People.Add(existingPerson);
        await _context.SaveChangesAsync();

        // Make multiple changes: add new people and update existing
        var newPerson1 = new Person("Jane", "Smith");
        var newPerson2 = new Person("Bob", "Johnson");
        
        _context.People.Add(newPerson1);
        _context.People.Add(newPerson2);
        
        existingPerson.FirstName = "John";
        existingPerson.LastName = "Updated";
        _context.People.Update(existingPerson);

        var countBeforeSave = await _context.People.CountAsync();
        Assert.Equal(1, countBeforeSave);

        // Act: Save all changes through repository
        await _personRepository.SaveChangesAsync();

        // Assert: Verify all changes were persisted
        var countAfterSave = await _context.People.CountAsync();
        Assert.Equal(3, countAfterSave);

        var updatedExisting = await _context.People.FirstAsync(p => p.Id == existingPerson.Id);
        Assert.Equal("John", updatedExisting.FirstName);
        Assert.Equal("Updated", updatedExisting.LastName);

        var allPeople = await _context.People.ToListAsync();
        Assert.Contains(allPeople, p => p.FirstName == newPerson1.FirstName && p.LastName == newPerson1.LastName);
        Assert.Contains(allPeople, p => p.FirstName == newPerson2.FirstName && p.LastName == newPerson2.LastName);
    }

    [Fact]
    public async Task AddAsync_Then_GetByIdAsync_Should_ReturnSamePerson()
    {
        var person = new Person("Joe", "Doe");
        var addedPerson = await _personRepository.AddAsync(person);

        var retrievedPerson = await _personRepository.GetByIdAsync(addedPerson.Id);

        Assert.NotNull(retrievedPerson);
        Assert.Equal(addedPerson.Id, retrievedPerson.Id);
        Assert.Equal(addedPerson.FirstName, retrievedPerson.FirstName);
        Assert.Equal(addedPerson.LastName, retrievedPerson.LastName);
    }

    [Fact]
    public async Task AddAsync_Then_GetAllAsync_Should_IncludeNewPerson()
    {
        var person = new Person("Joe", "Doe");
        var addedPerson = await _personRepository.AddAsync(person);

        var allPeople = await _personRepository.GetAllAsync();

        Assert.Single(allPeople);
        Assert.Contains(allPeople, p => p.Id == addedPerson.Id && 
                                        p.FirstName == person.FirstName && 
                                        p.LastName == person.LastName);
    }
}
