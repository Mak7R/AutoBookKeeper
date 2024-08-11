using AutoBookKeeper.Application.Models;
using AutoBookKeeper.Web.Models;
using AutoMapper;

namespace AutoBookKeeper.Web.Mappers;

public class WebProfile : Profile
{
    public WebProfile()
    {
        CreateUserMaps();   
    }

    public void CreateUserMaps()
    {
        CreateMap<UserModel, UserViewModel>().ReverseMap();
        CreateMap<UserModel, LoginDto>().ReverseMap();
        CreateMap<UserModel, RegisterDto>().ReverseMap();
    }
}