using AutoMapper;
using Moq;

namespace blog_api.UnitTests;

[TestFixture]
public class PostServiceTests
{
    private Mock<IUnitOfWork> uow;
    private IMapper mapper;
    private Mock<IPostRepository> postRepository;
    private Mock<IUserRepository> userRepository;

    [SetUp]
    public void Setup()
    {
        var mapperConfiguration = new MapperConfiguration(x => x.AddProfile<AutomapperProfile>());
        uow = new Mock<IUnitOfWork>();
        mapper = mapperConfiguration.CreateMapper();
        postRepository = new Mock<IPostRepository>();
        userRepository = new Mock<IUserRepository>();
    }

    [Test]
    public async Task CreateAsync_WithExistingUser_CreatesPost()
    {
        #region Arrange

        var user = new User() { Id = 1 };
        var createPostDto = new CreatePostDto() { Body = "b", Title = "T", UserId = user.Id };
        var post = new Post() { Body = createPostDto.Body, Title = createPostDto.Title, UserId = createPostDto.UserId };

        var _mapper = new Mock<IMapper>();
        _mapper.Setup(x => x.Map<Post>(createPostDto)).Returns(post);

        userRepository.Setup(x => x.GetByIdAsync(user.Id, "", false)).ReturnsAsync(user);

        uow.Setup(x => x.UserRepository).Returns(userRepository.Object);
        uow.Setup(x => x.PostRepository).Returns(postRepository.Object);

        #endregion

        #region Act

        var service = new PostService(uow.Object, _mapper.Object);
        await service.CreateAsync(createPostDto);

        #endregion

        postRepository.Verify(x => x.Create(_mapper.Object.Map<Post>(createPostDto)));
    }

    [Test]
    public void CreateAsync_WithNoUsers_ThrowsException()
    {
        #region Arrange

        string exceptionMessage = "User with provided Id was not found.";
        userRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), "", false)).ReturnsAsync(() => null);

        uow.Setup(x => x.UserRepository).Returns(userRepository.Object);
        uow.Setup(x => x.PostRepository).Returns(postRepository.Object);

        #endregion

        #region Act

        var service = new PostService(uow.Object, mapper);

        #endregion

        var exception = Assert.ThrowsAsync<Exception>(async () => await service.CreateAsync(new() { UserId = 1 }));
        Assert.That(exception.Message, Is.EqualTo(exceptionMessage));
    }
}
