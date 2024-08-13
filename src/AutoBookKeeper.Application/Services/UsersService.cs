using AutoBookKeeper.Application.Interfaces;
using AutoBookKeeper.Application.Mappers;
using AutoBookKeeper.Application.Models;
using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Models;
using AutoBookKeeper.Core.Repositories;
using AutoBookKeeper.Core.Specifications;
using Microsoft.Extensions.Logging;

namespace AutoBookKeeper.Application.Services;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<UsersService> _logger;

    public UsersService(IUsersRepository usersRepository, IPasswordHasher passwordHasher, ILogger<UsersService> logger)
    {
        _usersRepository = usersRepository;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }
    
    public async Task<UserModel?> GetByIdAsync(Guid userId)
    {
        var user = await _usersRepository.GetByIdAsync(userId);
        return ApplicationMapper.Mapper.Map<UserModel>(user);
    }

    public async Task<UserModel?> GetByUserNameAsync(string userName)
    {
        var users = await _usersRepository.GetAsync(new UserSpecification(u => u.UserName == userName));
        return ApplicationMapper.Mapper.Map<UserModel>(users.SingleOrDefault());
    }

    public async Task<UserModel?> GetByEmailAsync(string email)
    {
        var users = await _usersRepository.GetAsync(new UserSpecification(u => u.Email == email));
        return ApplicationMapper.Mapper.Map<UserModel>(users.FirstOrDefault());
    }

    public async Task<int> CountAsync()
    {
        return await _usersRepository.CountAsync();
    }

    public async Task<bool> VerifyPasswordAsync(UserModel user, string password)
    {
        var userEntity = await _usersRepository.GetByIdAsync(user.Id);
        if (userEntity == null)
            return false;

        return VerifyPassword(userEntity, password);
    }

    private bool VerifyPassword(User user, string password)
    {
        return _passwordHasher.VerifyPassword(user.PasswordHash, password);
    }

    public async Task<OperationResult<UserModel>> CreateAsync(UserModel user)
    {
        return await PrivateCreateUserAsync(user, string.Empty);
    }
    
    public async Task<OperationResult<UserModel>> CreateAsync(UserModel user, string password)
    {
        ArgumentException.ThrowIfNullOrEmpty(password);

        return await PrivateCreateUserAsync(user, _passwordHasher.HashPassword(password));
    }

    private async Task<OperationResult<UserModel>> PrivateCreateUserAsync(UserModel user, string passwordHash)
    {
        ArgumentNullException.ThrowIfNull(passwordHash);
        
        var userEntity = ApplicationMapper.Mapper.Map<User>(user);
        userEntity.PasswordHash = passwordHash;
        
        // TODO check is this userName already exists
        // TODO check is this email already exists
        
        var result = await _usersRepository.CreateAsync(userEntity);

        return ParseRepositoryResult(result);
    }

    public async Task<OperationResult<UserModel>> UpdateAsync(UserModel user)
    {
        var userEntity = await _usersRepository.GetByIdAsync(user.Id);
        if (userEntity == null) return ResultWhenUserNotFound();

        if (!string.IsNullOrWhiteSpace(user.UserName)) userEntity.UserName = user.UserName;
        if (!string.IsNullOrWhiteSpace(user.Email)) userEntity.Email = user.Email;
        
        // TODO check is this userName already exists
        // TODO check is this email already exists
        
        var result = await _usersRepository.UpdateAsync(userEntity);
        
        return ParseRepositoryResult(result);
    }

    public async Task<OperationResult<UserModel>> UpdateUserPassword(UserModel user, string currentPassword, string newPassword)
    {
        var userEntity = await _usersRepository.GetByIdAsync(user.Id);
        if (userEntity == null) return ResultWhenUserNotFound();

        if (!VerifyPassword(userEntity, currentPassword))
            return OperationResult<UserModel>.FromErrors(400, ["Invalid password"]);
        
        userEntity.PasswordHash = _passwordHasher.HashPassword(newPassword);
        
        var result = await _usersRepository.UpdateAsync(userEntity);
        
        return ParseRepositoryResult(result);
    }

    public async Task<OperationResult<UserModel>> DeleteAsync(UserModel user)
    {
        var userEntity = ApplicationMapper.Mapper.Map<User>(user);
        var result = await _usersRepository.DeleteAsync(userEntity);

        return ParseRepositoryResult(result);
    }

    private static OperationResult<UserModel> ParseRepositoryResult(OperationResult<User> repositoryResult) => 
        repositoryResult.ToOperationResult(ApplicationMapper.Mapper.Map<UserModel>);
    
    private static OperationResult<UserModel> ResultWhenUserNotFound() => 
        OperationResult<UserModel>.FromErrors(404, ["User was not found"]);
}