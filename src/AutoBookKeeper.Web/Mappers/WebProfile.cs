using AutoBookKeeper.Application.Models;
using AutoBookKeeper.Web.Models.Account;
using AutoBookKeeper.Web.Models.Book;
using AutoBookKeeper.Web.Models.Transaction;
using AutoBookKeeper.Web.Models.User;
using AutoMapper;

namespace AutoBookKeeper.Web.Mappers;

public class WebProfile : Profile
{
    public WebProfile()
    {
        CreateUserMaps();
        CreateBookMaps();
        CreateTransactionMaps();
    }

    private void CreateUserMaps()
    {
        CreateMap<UserModel, UserViewModel>().ReverseMap();
        CreateMap<UserModel, LoginDto>().ReverseMap();
        CreateMap<UserModel, RegisterDto>().ReverseMap();
        CreateMap<UserModel, UserProfileViewModel>().ReverseMap();
        CreateMap<UserModel, UpdateUserDto>().ReverseMap();
    }
    
    private void CreateBookMaps()
    {
        CreateMap<BookModel, BookViewModel>()
            .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.Owner.Id))
            .ReverseMap()
            .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => new UserModel{Id = src.OwnerId}));
        CreateMap<BookModel, CreateBookDto>().ReverseMap();
        CreateMap<BookModel, UpdateBookDto>().ReverseMap();
    }

    private void CreateTransactionMaps()
    {
        CreateMap<TransactionModel, TransactionViewModel>()
            .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.Book.Id))
            .ReverseMap()
            .ForMember(dest => dest.Book, opt => opt.MapFrom(src => new BookModel{Id = src.BookId}));
        CreateMap<TransactionModel, CreateTransactionDto>()
            .ReverseMap()
            .ForMember(
                dest => dest.TransactionTime, 
                opt => opt.MapFrom(src => src.TransactionTime ?? DateTime.UtcNow));
        CreateMap<TransactionModel, UpdateTransactionDto>()
            .ForMember(dest => dest.TransactionTime, opt => opt.MapFrom(
                src => src.TransactionTime.ToUniversalTime()))
            .ReverseMap()
            .ForMember(
                dest => dest.TransactionTime, 
                opt => opt.MapFrom(src => src.TransactionTime ?? DateTime.UtcNow));
    }
}