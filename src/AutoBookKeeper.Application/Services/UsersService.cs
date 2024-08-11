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

    public async Task<UserModel?> GetByNameAsync(string name)
    {
        var users = await _usersRepository.GetAsync(new UserSpecification(u => u.Name == name));
        return ApplicationMapper.Mapper.Map<UserModel>(users.SingleOrDefault());
    }

    public async Task<UserModel?> GetByEmailAsync(string email)
    {
        var users = await _usersRepository.GetAsync(new UserSpecification(u => u.Email == email));
        throw new NotImplementedException();
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

        return _passwordHasher.VerifyPassword(userEntity.PasswordHash, password);
    }

    public async Task<OperationResult<UserModel>> CreateUserAsync(UserModel user)
    {
        return await PrivateCreateUserAsync(user, string.Empty);
    }
    
    public async Task<OperationResult<UserModel>> CreateUserAsync(UserModel user, string password)
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

        if (result.IsSuccessful)
            return OperationResult<UserModel>.FromResult(ApplicationMapper.Mapper.Map<UserModel>(result.Result));
        return new OperationResult<UserModel>
        {
            Status = result.Status,
            Errors = result.Errors,
            Exception = result.Exception
        };
    }

    public Task<OperationResult<UserModel>> UpdateUserAsync(UserModel user)
    {
        throw new NotImplementedException();
    }

    public async Task<OperationResult<UserModel>> DeleteUserAsync(UserModel user)
    {
        var userEntity = ApplicationMapper.Mapper.Map<User>(user);
        var result = await _usersRepository.DeleteAsync(userEntity);

        if (result.IsSuccessful)
            return OperationResult<UserModel>.FromResult(user);
        return new OperationResult<UserModel>
        {
            Status = result.Status,
            Errors = result.Errors,
            Exception = result.Exception
        };
    }
}