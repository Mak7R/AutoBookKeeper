using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Repositories.Base;

namespace AutoBookKeeper.Core.Repositories;

public interface IBooksRepository : IRepository<Book, Guid>
{
        
}