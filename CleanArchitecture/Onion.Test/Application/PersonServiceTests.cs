using Microsoft.EntityFrameworkCore;
using Onion.Application.Interfaces;
using Onion.Application.Services;
using Onion.Domain.Requests;
using Onion.Domain.Responses;
using Onion.Infrastructure.Database;
using Onion.Infrastructure.Repositories;

namespace Onion.Test.Application;

public class PersonServiceTests
{
    private readonly OnionContext _context;
    private readonly IPersonService _personService;

    public PersonServiceTests()
    {
        var databaseName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<OnionContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;

        _context = new OnionContext(options);
        var personRepository = new PersonRepository(_context);
        _personService = new PersonService(personRepository);
    }

    [Fact]
    public async Task CreateAsync_Should_ReturnBasePersonResponse_When_PersonIsCreated()
    {
        var request = new CreatePersonRequest
        {
            FirstName = "Joe",
            LastName = "Doe"
        };

        var response = await _personService.CreateAsync(request);

        Assert.NotNull(response);
        Assert.NotEqual(0, response.Id);
        Assert.Equal(request.FirstName, response.FirstName);
        Assert.Equal(request.LastName, response.LastName);
    }

    [Fact]
    public async Task CreateAsync_Should_SavePersonToDatabase_When_PersonIsCreated()
    {
        var request = new CreatePersonRequest
        {
            FirstName = "Joe",
            LastName = "Doe"
        };

        await _personService.CreateAsync(request);

        var count = await _context.People.CountAsync();
        Assert.Equal(1, count);

        var savedPerson = await _context.People.SingleAsync();
        Assert.Equal(request.FirstName, savedPerson.FirstName);
        Assert.Equal(request.LastName, savedPerson.LastName);
    }

    [Fact]
    public async Task CreateAsync_Should_MapRequestCorrectlyToResponse_When_PersonIsCreated()
    {
        var request = new CreatePersonRequest
        {
            FirstName = "Jane",
            LastName = "Smith"
        };

        var response = await _personService.CreateAsync(request);

        Assert.Equal(request.FirstName, response.FirstName);
        Assert.Equal(request.LastName, response.LastName);
        Assert.True(response.Id > 0);
    }

    [Fact]
    public async Task ListAllAsync_Should_ReturnEmptyList_When_DatabaseIsEmpty()
    {
        var emptyList = await _personService.ListAllAsync();

        Assert.Empty(emptyList);
        Assert.Empty(_context.People);
    }

    [Fact]
    public async Task ListAllAsync_Should_ReturnAllPeople_When_PeopleExist()
    {
        int peopleQuantity = 5;
        var requests = new List<CreatePersonRequest>();

        for (int i = 0; i < peopleQuantity; i++)
        {
            var request = new CreatePersonRequest
            {
                FirstName = "Joe " + i,
                LastName = "Doe " + i
            };
            requests.Add(request);
            await _personService.CreateAsync(request);
        }

        var allPeople = await _personService.ListAllAsync();

        Assert.Equal(peopleQuantity, allPeople.Count);
        for (int i = 0; i < peopleQuantity; i++)
        {
            var person = allPeople.FirstOrDefault(p => p.FirstName == requests[i].FirstName);
            Assert.NotNull(person);
            Assert.Equal(requests[i].LastName, person.LastName);
        }
    }

    [Fact]
    public async Task ListAllAsync_Should_ReturnCorrectCount_When_MultiplePeopleExist()
    {
        int peopleQuantity = 10;

        for (int i = 0; i < peopleQuantity; i++)
        {
            var request = new CreatePersonRequest
            {
                FirstName = "Joe " + i,
                LastName = "Doe " + i
            };
            await _personService.CreateAsync(request);
        }

        var allPeople = await _personService.ListAllAsync();
        var countInDatabase = await _context.People.CountAsync();

        Assert.Equal(peopleQuantity, allPeople.Count);
        Assert.Equal(peopleQuantity, countInDatabase);
    }

    [Fact]
    public async Task ListOneByIdAsync_Should_ReturnBasePersonResponse_When_PersonExists()
    {
        var request = new CreatePersonRequest
        {
            FirstName = "Joe",
            LastName = "Doe"
        };
        var createdPerson = await _personService.CreateAsync(request);

        var retrievedPerson = await _personService.ListOneByIdAsync(createdPerson.Id);

        Assert.NotNull(retrievedPerson);
        Assert.Equal(createdPerson.Id, retrievedPerson.Id);
        Assert.Equal(request.FirstName, retrievedPerson.FirstName);
        Assert.Equal(request.LastName, retrievedPerson.LastName);
    }

    [Fact]
    public async Task ListOneByIdAsync_Should_ReturnNull_When_PersonDoesNotExist()
    {
        var nonExistentId = 999;

        var retrievedPerson = await _personService.ListOneByIdAsync(nonExistentId);

        Assert.Null(retrievedPerson);
    }

    [Fact]
    public async Task ListOneByIdAsync_Should_ReturnCorrectPerson_When_MultiplePeopleExist()
    {
        var request1 = new CreatePersonRequest { FirstName = "Joe", LastName = "Doe" };
        var request2 = new CreatePersonRequest { FirstName = "Jane", LastName = "Smith" };
        var request3 = new CreatePersonRequest { FirstName = "Bob", LastName = "Johnson" };

        var createdPerson1 = await _personService.CreateAsync(request1);
        var createdPerson2 = await _personService.CreateAsync(request2);
        var createdPerson3 = await _personService.CreateAsync(request3);

        var retrievedPerson = await _personService.ListOneByIdAsync(createdPerson2.Id);

        Assert.NotNull(retrievedPerson);
        Assert.Equal(createdPerson2.Id, retrievedPerson.Id);
        Assert.Equal(request2.FirstName, retrievedPerson.FirstName);
        Assert.Equal(request2.LastName, retrievedPerson.LastName);
    }

    [Fact]
    public async Task CreateAsync_Then_ListOneByIdAsync_Should_ReturnSamePerson()
    {
        var request = new CreatePersonRequest
        {
            FirstName = "Joe",
            LastName = "Doe"
        };
        var createdPerson = await _personService.CreateAsync(request);

        var retrievedPerson = await _personService.ListOneByIdAsync(createdPerson.Id);

        Assert.NotNull(retrievedPerson);
        Assert.Equal(createdPerson.Id, retrievedPerson.Id);
        Assert.Equal(createdPerson.FirstName, retrievedPerson.FirstName);
        Assert.Equal(createdPerson.LastName, retrievedPerson.LastName);
    }

    [Fact]
    public async Task CreateAsync_Then_ListAllAsync_Should_IncludeNewPerson()
    {
        var request = new CreatePersonRequest
        {
            FirstName = "Joe",
            LastName = "Doe"
        };
        var createdPerson = await _personService.CreateAsync(request);

        var allPeople = await _personService.ListAllAsync();

        Assert.Single(allPeople);
        Assert.Contains(allPeople, p => p.Id == createdPerson.Id &&
                                        p.FirstName == request.FirstName &&
                                        p.LastName == request.LastName);
    }
}
