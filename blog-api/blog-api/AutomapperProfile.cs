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
        CreateMap<UserModel, User>().ForMember(dst => dst.Address, opt => opt.MapFrom(src => src.Address.City + " " + src.Address.Street))
                                    .ForMember(dst => dst.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
                                    .ForMember(dst => dst.FirstName, opt => opt.MapFrom(src => GetFirstName(src.Name)))
                                    .ForMember(dst => dst.LastName, opt => opt.MapFrom(src => GetLastName(src.Name)));

        CreateMap<PostModel, Post>();
    }

    private string GetFirstName(string name)
    {
        var splittedName = name.Split(" ");
        return splittedName.Length == 3 ? $"{splittedName[0]} {splittedName[1]}" : $"{splittedName[0]}";
    }

    private string GetLastName(string name)
    {
        var splittedName = name.Split(" ");
        return splittedName.Length == 3 ? $"{splittedName[2]}" : $"{splittedName[1]}";
    }
}
