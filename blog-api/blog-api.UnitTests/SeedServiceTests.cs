using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Moq;

namespace blog_api.UnitTests;

[TestFixture]
public class SeedServiceTests
{
    private Mock<IUnitOfWork> uow;
    private Mock<IJsonPlaceholderService> jsonPlaceHolderService;
    private Mock<IUserRepository> userRepository;
    private IMapper mapper;
    private SeedService seedService;

    [SetUp]
    public void SetUp()
    {
        uow = new Mock<IUnitOfWork>();
        jsonPlaceHolderService = new Mock<IJsonPlaceholderService>();
        userRepository = new Mock<IUserRepository>();

        uow.Setup(x => x.UserRepository).Returns(userRepository.Object);

        var mapperConfiguration = new MapperConfiguration(x => x.AddProfile<AutomapperProfile>());
        mapper = mapperConfiguration.CreateMapper();

        seedService = new SeedService(jsonPlaceHolderService.Object, uow.Object, mapper);
    }

    #region NEGATIVE_TESTS

    [Test]
    public async Task Seed_ThereOneUserInDatabase_DoesNotCreateUsers()
    {
        userRepository.Setup(x => x.GetAllAsync(null!, "", false)).ReturnsAsync([new User()]);
        await seedService.SeedDatabaseAsync();

        userRepository.Verify(x => x.Create(It.IsAny<User>()), Times.Never());
    }

    [Test]
    public async Task Seed_ThereIsNoUserInDatabase_CreatesUsers()
    {
        userRepository.Setup(x => x.GetAllAsync(null!, "", false)).ReturnsAsync([]);

        jsonPlaceHolderService.Setup(x => x.FetchUsersAsync()).ReturnsAsync(FakeData.GetUsers());
        jsonPlaceHolderService.Setup(x => x.FetchPostsAsync()).ReturnsAsync(FakeData.GetPosts());

        await seedService.SeedDatabaseAsync();

        userRepository.Verify(x => x.Create(It.IsAny<List<User>>()), Times.Once());
    }

    #endregion
}
