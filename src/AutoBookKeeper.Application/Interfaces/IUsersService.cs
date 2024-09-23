using AutoBookKeeper.Application.Models;
using AutoBookKeeper.Core.Models;

namespace AutoBookKeeper.Application.Interfaces;

public interface IUsersService
{
    Task<UserModel?> GetByIdAsync(Guid userId);
    Task<UserModel?> GetByUserNameAsync(string userName);
    Task<UserModel?> GetByEmailAsync(string email);

    Task<int> CountAsync();
    Task<bool> VerifyPasswordAsync(UserModel user, string password);
    
    Task<OperationResult<UserModel>> CreateAsync(UserModel user);
    Task<OperationResult<UserModel>> CreateAsync(UserModel user, string password);
    Task<OperationResult<UserModel>> UpdateUserPassword(UserModel user, string currentPassword, string newPassword);
    Task<OperationResult<UserModel>> UpdateAsync(UserModel user);
    Task<OperationResult<UserModel>> DeleteAsync(UserModel user);
}