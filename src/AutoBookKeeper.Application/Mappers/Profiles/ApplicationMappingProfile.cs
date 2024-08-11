using AutoBookKeeper.Application.Models;
using AutoBookKeeper.Core.Entities;
using AutoMapper;

namespace AutoBookKeeper.Application.Mappers.Profiles;

public class ApplicationMappingProfile : Profile
{
    public ApplicationMappingProfile()
    {
        CreateUserMaps();
    }

    private void CreateUserMaps()
    {
        CreateMap<User, UserModel>()
            .ReverseMap();
    }
}