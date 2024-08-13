using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Repositories;
using AutoBookKeeper.Infrastructure.Data;
using AutoBookKeeper.Infrastructure.Repositories.Base;
using Microsoft.Extensions.Logging;

namespace AutoBookKeeper.Infrastructure.Repositories;

public class BooksRepository : Repository<Book, Guid>, IBooksRepository
{
    public BooksRepository(ApplicationDbContext dbContext, ILogger<Repository<Book, Guid>> logger) : base(dbContext, logger)
    {
    }
}