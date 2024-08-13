using AutoBookKeeper.Application.Models;
using AutoBookKeeper.Web.Models.Book;
using AutoBookKeeper.Web.Models.User;
using AutoMapper;

namespace AutoBookKeeper.Web.Mappers;

public class WebProfile : Profile
{
    public WebProfile()
    {
        CreateUserMaps();
        CreateBookMaps();
    }

    private void CreateBookMaps()
    {
        CreateMap<BookModel, BookViewModel>()
            .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.Owner.Id))
            .ReverseMap()
            .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => new UserModel{Id = src.OwnerId}));
        CreateMap<BookModel, CreateBookDto>().ReverseMap();
    }

    public void CreateUserMaps()
    {
        CreateMap<UserModel, UserViewModel>().ReverseMap();
        CreateMap<UserModel, LoginDto>().ReverseMap();
        CreateMap<UserModel, RegisterDto>().ReverseMap();
        CreateMap<UserModel, UserProfileViewModel>().ReverseMap();
        CreateMap<UserModel, UpdateUserDto>().ReverseMap();
    }
}