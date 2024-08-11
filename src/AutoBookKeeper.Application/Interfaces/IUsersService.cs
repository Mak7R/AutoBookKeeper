using AutoBookKeeper.Application.Models;
using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Models;

namespace AutoBookKeeper.Application.Interfaces;

public interface IUsersService
{
    public Task<User?> GetByIdAsync(Guid userId);
    public Task<User?> GetByNameAsync(string name);
    public Task<User?> GetByEmailAsync(string email);

    public Task<int> CountAsync();
    public Task<bool> ConfirmPasswordAsync(UserModel user, string password);
    
    public Task<OperationResult> CreateUserAsync(UserModel user);
    public Task<OperationResult> UpdateUserAsync(UserModel user);
    public Task<OperationResult> DeleteUserAsync(UserModel user);
}