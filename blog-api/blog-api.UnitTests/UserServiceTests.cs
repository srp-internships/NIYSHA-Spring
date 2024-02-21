using System.Linq.Expressions;
using AutoMapper;
using Moq;

namespace blog_api.UnitTests;

[TestFixture]
public class UserServiceTests
{
    private Mock<IUnitOfWork> uow;
    private IMapper mapper;
    private Mock<IUserRepository> userRepository;

    [SetUp]
    public void Setup()
    {
        uow = new Mock<IUnitOfWork>();
        var mapperConfiguration = new MapperConfiguration(x => x.AddProfile<AutomapperProfile>());
        mapper = mapperConfiguration.CreateMapper();
        userRepository = new Mock<IUserRepository>();
    }

    [Test]
    public async Task GetAll_WhenCalled_ReturnsExpectedResult()
    {
        userRepository.Setup(x => x.GetAllAsync(null!, "", true)).ReturnsAsync(FakeData.GetUsers());
        uow.Setup(x => x.UserRepository).Returns(userRepository.Object);
        var expectedData = mapper.Map<List<UserDto>>(FakeData.GetUsers());

        var service = new UserService(uow.Object, mapper);
        var result = await service.GetAllAsync();

        Assert.IsTrue(result.SequenceEqual(expectedData, new UserDtoComparer()));
        Assert.That(expectedData, Has.Count.EqualTo(result.Count));
        Assert.That(result[0].Id, Is.EqualTo(expectedData[0].Id));
    }

    [Test]
    public async Task Search_ThreeOccurrence_ReturnThreeUsers()
    {
        var users = new List<User>()
        {
            new User() { Id = 1, FirstName = "Emily", LastName = "Johnson", Username = "emily990" },
            new User() { Id = 2, FirstName = "Michael", LastName = "Davis", Username = "m.davis" },
            new User() { Id = 3, FirstName = "Jackson", LastName = "Miller", Username = "miller" }
        };

        userRepository.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<User, bool>>>(), "", true)).ReturnsAsync(users);
        uow.Setup(x => x.UserRepository).Returns(() => userRepository.Object);
        var expectedData = mapper.Map<List<UserDto>>(users);

        var service = new UserService(uow.Object, mapper);
        var result = await service.SearchAsync("mi");

        Assert.IsTrue(expectedData.SequenceEqual(result, new UserDtoComparer()));
    }

    [Test]
    public async Task Search_ZeroOccurrence_ReturnEmptyList()
    {
        var users = new List<User>()
        {
            new User() { Id = 1, FirstName = "Emily", LastName = "Johnson", Username = "emily990" },
            new User() { Id = 2, FirstName = "Michael", LastName = "Davis", Username = "m.davis" },
            new User() { Id = 4, FirstName = "Jackson", LastName = "Miller", Username = "miller" }
        };

        userRepository.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<User, bool>>>(), "", true)).ReturnsAsync([]);
        uow.Setup(x => x.UserRepository).Returns(() => userRepository.Object);
        var expectedData = mapper.Map<List<UserDto>>(users);

        var service = new UserService(uow.Object, mapper);
        var result = await service.SearchAsync("xy");

        Assert.IsFalse(expectedData.SequenceEqual(result, new UserDtoComparer()));
        Assert.That(result, Has.Count.EqualTo(0));
    }
}
