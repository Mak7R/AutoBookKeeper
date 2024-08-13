using AutoBookKeeper.Application.Models;
using AutoBookKeeper.Core.Entities;
using AutoMapper;

namespace AutoBookKeeper.Application.Mappers.Profiles;

public class ApplicationMappingProfile : Profile
{
    public ApplicationMappingProfile()
    {
        CreateUserMaps();
        CreateBookMaps();
    }

    private void CreateBookMaps()
    {
        CreateMap<Book, BookModel>()
            .AfterMap((src, dest) => { dest.Owner ??= new UserModel { Id = src.OwnerId }; })
            .ReverseMap()
            .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.Owner.Id))
            .ForMember(dest => dest.Owner, opt => opt.Ignore());
    }

    private void CreateUserMaps()
    {
        CreateMap<User, UserModel>()
            .ReverseMap();
    }
}