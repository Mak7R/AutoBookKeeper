using AutoBookKeeper.Application.Exceptions;
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
        return user == null ? null : ApplicationMapper.Mapper.Map<UserModel>(user);
    }

    public async Task<UserModel?> GetByUserNameAsync(string userName)
    {
        var users = await _usersRepository.GetAsync(UserSpecification.GetByName(userName));
        return ApplicationMapper.Mapper.Map<UserModel>(users.SingleOrDefault());
    }

    public async Task<UserModel?> GetByEmailAsync(string email)
    {
        var users = await _usersRepository.GetAsync(UserSpecification.GetByEmail(email));
        return ApplicationMapper.Mapper.Map<UserModel>(users.SingleOrDefault());
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

        if ((await _usersRepository.GetAsync(UserSpecification.GetByName(user.UserName))).Any())
            return AlreadyExistsResult("User with this name already exists");
        
        // todo check is this email already exists
        
        var result = await _usersRepository.CreateAsync(userEntity);

        return MappedRepositoryResult(result);
    }

    public async Task<OperationResult<UserModel>> UpdateAsync(UserModel user)
    {
        var userEntity = await _usersRepository.GetByIdAsync(user.Id);
        if (userEntity == null) return NotFoundResult();

        if (!string.IsNullOrWhiteSpace(user.UserName))
        {
            userEntity.UserName = user.UserName;
            
            if ((await _usersRepository.GetAsync(UserSpecification.GetByName(user.UserName))).Any())
                return AlreadyExistsResult("User with this name already exists");
        }

        if (!string.IsNullOrWhiteSpace(user.Email))
        {
            userEntity.Email = user.Email;
            
            // TODO check is new email already exists
        }
        
        var result = await _usersRepository.UpdateAsync(userEntity);
        
        return MappedRepositoryResult(result);
    }

    public async Task<OperationResult<UserModel>> UpdateUserPassword(UserModel user, string currentPassword, string newPassword)
    {
        var userEntity = await _usersRepository.GetByIdAsync(user.Id);
        if (userEntity == null) return NotFoundResult();

        if (!VerifyPassword(userEntity, currentPassword))
            return OperationResult<UserModel>.FromErrors(400, ["Invalid password"]);
        
        userEntity.PasswordHash = _passwordHasher.HashPassword(newPassword);
        
        var result = await _usersRepository.UpdateAsync(userEntity);
        
        return MappedRepositoryResult(result);
    }

    public async Task<OperationResult<UserModel>> DeleteAsync(UserModel user)
    {
        var userEntity = await _usersRepository.GetByIdAsync(user.Id);
        if (userEntity == null) return NotFoundResult();
        
        var result = await _usersRepository.DeleteAsync(userEntity);

        return MappedRepositoryResult(result);
    }

    private static OperationResult<UserModel> MappedRepositoryResult(OperationResult<User> repositoryResult) => 
        repositoryResult.ToOperationResult(ApplicationMapper.Mapper.Map<UserModel>);
    
    private static OperationResult<UserModel> NotFoundResult(params string[] errors) => 
        errors.Length > 0 ? 
            new OperationResult<UserModel> {Status = 404, Exception = new NotFoundException(errors[0]), Errors = errors} : 
            new OperationResult<UserModel> {Status = 404, Exception = new NotFoundException("User was not found"), Errors = ["User was not found"]};

    private static OperationResult<UserModel> AlreadyExistsResult(params string[] errors) => 
        errors.Length > 0 ? 
            new OperationResult<UserModel> {Status = 400, Exception = new AlreadyExistsException(errors[0]), Errors = errors} : 
            new OperationResult<UserModel> {Status = 400, Exception = new AlreadyExistsException("User already exists"), Errors = ["User already exists"]};
}