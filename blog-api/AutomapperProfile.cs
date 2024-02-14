using AutoMapper;

namespace blog_api;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        CreateMap<CreatePostDto, Post>();
        CreateMap<Post, PostDto>().ForMember(x => x.UserFullName, opt => opt.MapFrom(src => src.User.FullName));
        CreateMap<UpdatePostDto, Post>();
        CreateMap<User, UserDto>();
    }
}
