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
        CreateTransactionMaps();
        CreateTransactionTypeMaps();
        CreateRoleMaps();
    }

    private void CreateRoleMaps()
    {
        CreateMap<BookRole, RoleModel>().ReverseMap();
    }

    private void CreateTransactionTypeMaps()
    {
        CreateMap<TransactionType, TransactionTypeModel>()
            .ReverseMap()
            .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.Book.Id))
            .ForMember(dest => dest.Book, opt => opt.Ignore());
    }

    private void CreateTransactionMaps()
    {
        CreateMap<Transaction, TransactionModel>()
            .AfterMap((src, dest) => { dest.Book ??= new BookModel { Id = src.BookId }; })
            .ReverseMap()
            .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.Book.Id))
            .ForMember(dest => dest.Book, opt => opt.Ignore());
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