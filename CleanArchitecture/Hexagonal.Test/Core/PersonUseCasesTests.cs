using Hexagonal.Core.Application.UseCases;
using Hexagonal.Core.Domain.Requests;
using Hexagonal.Core.Domain.Responses;
using Hexagonal.Infrastructure.Persistence.Database;
using Hexagonal.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace Hexagonal.Test.Core;

public class PersonUseCasesTests
{
    private readonly HexagonalContext _context;
    private readonly CreatePersonUseCase _createPersonUseCase;
    private readonly GetAllPersonsUseCase _getAllPersonsUseCase;
    private readonly GetPersonByIdUseCase _getPersonByIdUseCase;

    public PersonUseCasesTests()
    {
        var databaseName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<HexagonalContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;

        _context = new HexagonalContext(options);
        var personRepository = new PersonRepository(_context);
        _createPersonUseCase = new CreatePersonUseCase(personRepository);
        _getAllPersonsUseCase = new GetAllPersonsUseCase(personRepository);
        _getPersonByIdUseCase = new GetPersonByIdUseCase(personRepository);
    }

    #region CreatePersonUseCase Tests

    [Fact]
    public async Task CreatePersonUseCase_ExecuteAsync_Should_ReturnPersonResponse_When_PersonIsCreated()
    {
        // Arrange
        var request = new CreatePersonRequest
        {
            FirstName = "Joe",
            LastName = "Doe"
        };

        // Act
        var response = await _createPersonUseCase.ExecuteAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(request.FirstName, response.FirstName);
        Assert.Equal(request.LastName, response.LastName);
        Assert.NotEqual(0, response.Id);
    }

    [Fact]
    public async Task CreatePersonUseCase_ExecuteAsync_Should_SavePersonToDatabase_When_PersonIsCreated()
    {
        // Arrange
        var request = new CreatePersonRequest
        {
            FirstName = "Joe",
            LastName = "Doe"
        };

        // Act
        var response = await _createPersonUseCase.ExecuteAsync(request);

        // Assert
        var savedPerson = await _context.People.SingleAsync();
        Assert.Single(_context.People);
        Assert.Equal(request.FirstName, savedPerson.FirstName);
        Assert.Equal(request.LastName, savedPerson.LastName);
        Assert.Equal(response.Id, savedPerson.Id);
    }

    [Fact]
    public async Task CreatePersonUseCase_ExecuteAsync_Should_AssignUniqueIds_When_MultiplePeopleAreCreated()
    {
        // Arrange
        var request1 = new CreatePersonRequest { FirstName = "Joe", LastName = "Doe" };
        var request2 = new CreatePersonRequest { FirstName = "Jane", LastName = "Smith" };

        // Act
        var response1 = await _createPersonUseCase.ExecuteAsync(request1);
        var response2 = await _createPersonUseCase.ExecuteAsync(request2);

        // Assert
        Assert.NotEqual(response1.Id, response2.Id);
        Assert.NotEqual(0, response1.Id);
        Assert.NotEqual(0, response2.Id);
        Assert.Equal(2, await _context.People.CountAsync());
    }

    [Fact]
    public async Task CreatePersonUseCase_ExecuteAsync_Should_CreatePersonWithCorrectData_When_RequestIsValid()
    {
        // Arrange
        var request = new CreatePersonRequest
        {
            FirstName = "John",
            LastName = "Doe"
        };

        // Act
        var response = await _createPersonUseCase.ExecuteAsync(request);

        // Assert
        var savedPerson = await _context.People.FirstAsync(p => p.Id == response.Id);
        Assert.Equal("John", savedPerson.FirstName);
        Assert.Equal("Doe", savedPerson.LastName);
        Assert.Equal("John", response.FirstName);
        Assert.Equal("Doe", response.LastName);
    }

    #endregion

    #region GetAllPersonsUseCase Tests

    [Fact]
    public async Task GetAllPersonsUseCase_ExecuteAsync_Should_ReturnEmptyList_When_DatabaseIsEmpty()
    {
        // Act
        var result = await _getAllPersonsUseCase.ExecuteAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        Assert.Empty(_context.People);
    }

    [Fact]
    public async Task GetAllPersonsUseCase_ExecuteAsync_Should_ReturnAllPeople_When_PeopleExist()
    {
        // Arrange
        var request1 = new CreatePersonRequest { FirstName = "Joe", LastName = "Doe" };
        var request2 = new CreatePersonRequest { FirstName = "Jane", LastName = "Smith" };
        var request3 = new CreatePersonRequest { FirstName = "Bob", LastName = "Johnson" };

        await _createPersonUseCase.ExecuteAsync(request1);
        await _createPersonUseCase.ExecuteAsync(request2);
        await _createPersonUseCase.ExecuteAsync(request3);

        // Act
        var result = await _getAllPersonsUseCase.ExecuteAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Contains(result, p => p.FirstName == "Joe" && p.LastName == "Doe");
        Assert.Contains(result, p => p.FirstName == "Jane" && p.LastName == "Smith");
        Assert.Contains(result, p => p.FirstName == "Bob" && p.LastName == "Johnson");
    }

    [Fact]
    public async Task GetAllPersonsUseCase_ExecuteAsync_Should_ReturnCorrectCount_When_MultiplePeopleExist()
    {
        // Arrange
        int peopleQuantity = 10;

        for (int i = 0; i < peopleQuantity; i++)
        {
            var request = new CreatePersonRequest
            {
                FirstName = "Joe " + i,
                LastName = "Doe " + i
            };
            await _createPersonUseCase.ExecuteAsync(request);
        }

        // Act
        var result = await _getAllPersonsUseCase.ExecuteAsync();

        // Assert
        Assert.Equal(peopleQuantity, result.Count);
        Assert.Equal(peopleQuantity, await _context.People.CountAsync());
    }

    [Fact]
    public async Task GetAllPersonsUseCase_ExecuteAsync_Should_ReturnBasePersonResponse_When_PeopleExist()
    {
        // Arrange
        var request = new CreatePersonRequest { FirstName = "Joe", LastName = "Doe" };
        var createdResponse = await _createPersonUseCase.ExecuteAsync(request);

        // Act
        var result = await _getAllPersonsUseCase.ExecuteAsync();

        // Assert
        Assert.Single(result);
        var personResponse = result.First();
        Assert.Equal(createdResponse.Id, personResponse.Id);
        Assert.Equal(createdResponse.FirstName, personResponse.FirstName);
        Assert.Equal(createdResponse.LastName, personResponse.LastName);
    }

    #endregion

    #region GetPersonByIdUseCase Tests

    [Fact]
    public async Task GetPersonByIdUseCase_ExecuteAsync_Should_ReturnPerson_When_PersonExists()
    {
        // Arrange
        var request = new CreatePersonRequest { FirstName = "Joe", LastName = "Doe" };
        var createdResponse = await _createPersonUseCase.ExecuteAsync(request);

        // Act
        var result = await _getPersonByIdUseCase.ExecuteAsync(createdResponse.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createdResponse.Id, result.Id);
        Assert.Equal(createdResponse.FirstName, result.FirstName);
        Assert.Equal(createdResponse.LastName, result.LastName);
    }

    [Fact]
    public async Task GetPersonByIdUseCase_ExecuteAsync_Should_ReturnNull_When_PersonDoesNotExist()
    {
        // Arrange
        var nonExistentId = 999;

        // Act
        var result = await _getPersonByIdUseCase.ExecuteAsync(nonExistentId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetPersonByIdUseCase_ExecuteAsync_Should_ReturnCorrectPerson_When_MultiplePeopleExist()
    {
        // Arrange
        var request1 = new CreatePersonRequest { FirstName = "Joe", LastName = "Doe" };
        var request2 = new CreatePersonRequest { FirstName = "Jane", LastName = "Smith" };
        var request3 = new CreatePersonRequest { FirstName = "Bob", LastName = "Johnson" };

        var createdResponse1 = await _createPersonUseCase.ExecuteAsync(request1);
        var createdResponse2 = await _createPersonUseCase.ExecuteAsync(request2);
        var createdResponse3 = await _createPersonUseCase.ExecuteAsync(request3);

        // Act
        var result = await _getPersonByIdUseCase.ExecuteAsync(createdResponse2.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createdResponse2.Id, result.Id);
        Assert.Equal("Jane", result.FirstName);
        Assert.Equal("Smith", result.LastName);
    }

    [Fact]
    public async Task GetPersonByIdUseCase_ExecuteAsync_Should_ReturnBasePersonResponse_When_PersonExists()
    {
        // Arrange
        var request = new CreatePersonRequest { FirstName = "John", LastName = "Doe" };
        var createdResponse = await _createPersonUseCase.ExecuteAsync(request);

        // Act
        var result = await _getPersonByIdUseCase.ExecuteAsync(createdResponse.Id);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BasePersonResponse>(result);
        Assert.Equal("John", result.FirstName);
        Assert.Equal("Doe", result.LastName);
    }

    [Fact]
    public async Task GetPersonByIdUseCase_ExecuteAsync_Should_ReturnNull_When_IdIsZero()
    {
        // Act
        var result = await _getPersonByIdUseCase.ExecuteAsync(0);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetPersonByIdUseCase_ExecuteAsync_Should_ReturnNull_When_IdIsNegative()
    {
        // Act
        var result = await _getPersonByIdUseCase.ExecuteAsync(-1);

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region Integration Tests

    [Fact]
    public async Task CreatePerson_Then_GetById_Should_ReturnSamePerson()
    {
        // Arrange
        var request = new CreatePersonRequest { FirstName = "Joe", LastName = "Doe" };
        var createdResponse = await _createPersonUseCase.ExecuteAsync(request);

        // Act
        var retrievedResponse = await _getPersonByIdUseCase.ExecuteAsync(createdResponse.Id);

        // Assert
        Assert.NotNull(retrievedResponse);
        Assert.Equal(createdResponse.Id, retrievedResponse.Id);
        Assert.Equal(createdResponse.FirstName, retrievedResponse.FirstName);
        Assert.Equal(createdResponse.LastName, retrievedResponse.LastName);
    }

    [Fact]
    public async Task CreatePerson_Then_GetAll_Should_IncludeNewPerson()
    {
        // Arrange
        var request = new CreatePersonRequest { FirstName = "Joe", LastName = "Doe" };
        var createdResponse = await _createPersonUseCase.ExecuteAsync(request);

        // Act
        var allPeople = await _getAllPersonsUseCase.ExecuteAsync();

        // Assert
        Assert.Single(allPeople);
        Assert.Contains(allPeople, p => p.Id == createdResponse.Id &&
                                        p.FirstName == request.FirstName &&
                                        p.LastName == request.LastName);
    }

    [Fact]
    public async Task CreateMultiplePeople_Then_GetAll_Should_ReturnAllPeople()
    {
        // Arrange
        var requests = new List<CreatePersonRequest>
        {
            new() { FirstName = "Joe", LastName = "Doe" },
            new() { FirstName = "Jane", LastName = "Smith" },
            new() { FirstName = "Bob", LastName = "Johnson" }
        };

        var createdResponses = new List<BasePersonResponse>();
        foreach (var request in requests)
        {
            var response = await _createPersonUseCase.ExecuteAsync(request);
            createdResponses.Add(response);
        }

        // Act
        var allPeople = await _getAllPersonsUseCase.ExecuteAsync();

        // Assert
        Assert.Equal(3, allPeople.Count);
        foreach (var createdResponse in createdResponses)
        {
            Assert.Contains(allPeople, p => p.Id == createdResponse.Id);
        }
    }

    #endregion
}
