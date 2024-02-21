using System.Linq.Expressions;
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
        var service = new PostService(uow.Object, _mapper.Object);

        #endregion

        await service.CreateAsync(createPostDto);

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
        var service = new PostService(uow.Object, mapper);

        #endregion

        var exception = Assert.ThrowsAsync<Exception>(async () => await service.CreateAsync(new() { UserId = 1 }));
        Assert.That(exception.Message, Is.EqualTo(exceptionMessage));
    }

    [Test]
    public async Task DeleteAsync_WithExistingPost_DeletesPassedPost()
    {
        var post = new Post() { Id = 1, Body = "b", Title = "T", UserId = 1 };

        uow.Setup(x => x.PostRepository).Returns(postRepository.Object);
        postRepository.Setup(x => x.GetByIdAsync(1, "", false)).ReturnsAsync(post);
        var service = new PostService(uow.Object, mapper);

        await service.DeleteAsync(post.Id);

        postRepository.Verify(x => x.Delete(post));
    }

    [Test]
    public void DeleteAsync_WithNoPosts_ThrowsException()
    {
        var exceptionMessage = "Post with provided Id was not found.";

        uow.Setup(x => x.PostRepository).Returns(postRepository.Object);
        postRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), "", false)).ReturnsAsync(() => null);
        var service = new PostService(uow.Object, mapper);

        var exception = Assert.ThrowsAsync<Exception>(async () => await service.DeleteAsync(It.IsAny<int>()));

        Assert.That(exception.Message, Is.EqualTo(exceptionMessage));
    }

    [Test]
    public async Task GetAllAsync_WhenCalled_ReturnPostDtos()
    {
        #region Arrange

        var posts = new List<Post>()
        {
            new Post() { Id = 1, Body = "B", Title = "T", UserId = 1},
            new Post() { Id = 2, Body = "B1", Title = "T1", UserId = 2}
        };

        var expected = mapper.Map<List<PostDto>>(posts);
        uow.Setup(x => x.PostRepository).Returns(postRepository.Object);
        postRepository.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Post, bool>>>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(posts);

        var service = new PostService(uow.Object, mapper);

        #endregion

        var result = await service.GetAllAsync();

        Assert.IsTrue(expected.SequenceEqual(result, new PostDtoComparer()));
    }

    [Test]
    public async Task GetPagingAsync_WhenCalled_ReturnsPostDtos()
    {
        #region Arrange

        var posts = new List<Post>()
        {
            new() {Id = 1, Body = "B1", Title = "T1", UserId = 1},
            new() {Id = 2, Body = "B2", Title = "T2", UserId = 2},
            new() {Id = 3, Body = "B3", Title = "T3", UserId = 3},
            new() {Id = 4, Body = "B4", Title = "T4", UserId = 4}
        };

        var expected = mapper.Map<List<PostDto>>(posts);

        postRepository.Setup(x => x.GetPagedList(4, 1, true)).ReturnsAsync(posts);
        uow.Setup(x => x.PostRepository).Returns(postRepository.Object);

        var service = new PostService(uow.Object, mapper);

        #endregion

        var result = await service.GetPagingAsync(4, 1);

        Assert.IsTrue(expected.SequenceEqual(result, new PostDtoComparer()));
    }

    [Test]
    public async Task GetUserPostsAsync_WhenCalled_ReturnsUserPosts()
    {
        var user = new User() { Id = 1 };
        var posts = new List<Post>()
        {
            new() {Id = 1, Body = "B1", Title = "T1", User = user, UserId = 1},
            new() {Id = 2, Body = "B2", Title = "T2", User = user, UserId = 1}
        };
        var expected = mapper.Map<List<PostDto>>(posts);

        postRepository.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Post, bool>>>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(posts);
        uow.Setup(x => x.PostRepository).Returns(postRepository.Object);

        var service = new PostService(uow.Object, mapper);
        var result = await service.GetUserPostsAsync(1);

        Assert.IsTrue(expected.SequenceEqual(result, new PostDtoComparer()));
    }

    [Test]
    public async Task UpdateAsync_WithCorrectPostId_UpdatesPost()
    {
        #region Arrange

        var post = new Post() { Id = 1, Title = "Hello world", Body = "Hello world 1", UserId = 2 };
        var updatePostDto = new UpdatePostDto() { Id = post.Id, Body = post.Body, Title = post.Title };
        postRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(post);
        uow.Setup(x => x.PostRepository).Returns(postRepository.Object);
        var mockedMapper = new Mock<IMapper>();
        mockedMapper.Setup(x => x.Map<UpdatePostDto>(post)).Returns(updatePostDto);

        var service = new PostService(uow.Object, mockedMapper.Object);

        #endregion

        await service.UpdateAsync(mockedMapper.Object.Map<UpdatePostDto>(post));

        mockedMapper.Verify(x => x.Map(updatePostDto, post));
    }

    [Test]
    public void UpdateAsync_WithIncorrrectPostId_ThrowsException()
    {
        postRepository.Setup(x => x.GetByIdAsync(1, It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(() => null);
        uow.Setup(x => x.PostRepository).Returns(postRepository.Object);

        var service = new PostService(uow.Object, mapper);

        Assert.ThrowsAsync<Exception>(async () => await service.UpdateAsync(new()));
    }
}