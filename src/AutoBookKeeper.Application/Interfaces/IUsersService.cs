using AutoBookKeeper.Application.Models;
using AutoBookKeeper.Core.Models;

namespace AutoBookKeeper.Application.Interfaces;

public interface IUsersService
{
    public Task<UserModel?> GetByIdAsync(Guid userId);
    public Task<UserModel?> GetByNameAsync(string name);
    public Task<UserModel?> GetByEmailAsync(string email);

    public Task<int> CountAsync();
    public Task<bool> VerifyPasswordAsync(UserModel user, string password);
    
    public Task<OperationResult<UserModel>> CreateUserAsync(UserModel user);
    public Task<OperationResult<UserModel>> CreateUserAsync(UserModel user, string password);
    public Task<OperationResult<UserModel>> UpdateUserAsync(UserModel user);
    public Task<OperationResult<UserModel>> DeleteUserAsync(UserModel user);
}