using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Repositories.Base;

namespace AutoBookKeeper.Core.Repositories;

public interface IUserTokensRepository : IRepository<UserToken, int>
{
    Task RemoveExpiredTokens();
}