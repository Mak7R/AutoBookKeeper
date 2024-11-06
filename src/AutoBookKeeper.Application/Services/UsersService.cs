using AutoBookKeeper.Application.Extensions;
using AutoBookKeeper.Application.Helpers;
using AutoBookKeeper.Application.Interfaces;
using AutoBookKeeper.Application.Mappers;
using AutoBookKeeper.Application.Models;
using AutoBookKeeper.Application.Validators;
using AutoBookKeeper.Core.Entities;
using AutoBookKeeper.Core.Models;
using AutoBookKeeper.Core.Repositories;
using AutoBookKeeper.Core.Specifications;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace AutoBookKeeper.Application.Services;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IValidator<UserModel> _userValidator;
    private readonly IValidator<Password> _passwordValidator;
    private readonly ILogger<UsersService> _logger;

    public UsersService(IUsersRepository usersRepository, IPasswordHasher passwordHasher, IValidator<UserModel> userValidator, IValidator<Password> passwordValidator, ILogger<UsersService> logger)
    {
        _usersRepository = usersRepository;
        _passwordHasher = passwordHasher;
        _userValidator = userValidator;
        _passwordValidator = passwordValidator;
        _logger = logger;
    }
    
    public async Task<UserModel?> GetByIdAsync(Guid userId)
    {
        try
        {
            var user = await _usersRepository.GetByIdAsync(userId);
            return user == null ? null : ApplicationMapper.Mapper.Map<UserModel>(user);
        }
        catch (Exception e)
        {
            throw ApplicationExceptionsHandlingHelper.HandleRetrieveDataException(e, _logger);
        }
    }

    public async Task<UserModel?> GetByUserNameAsync(string userName)
    {
        try
        {
            var users = await _usersRepository.GetAsync(UserSpecification.GetByName(userName));
            return ApplicationMapper.Mapper.Map<UserModel>(users.SingleOrDefault());
        }
        catch (Exception e)
        {
            throw ApplicationExceptionsHandlingHelper.HandleRetrieveDataException(e, _logger);
        }
    }

    public async Task<UserModel?> GetByEmailAsync(string email)
    {
        try
        {
            var users = await _usersRepository.GetAsync(UserSpecification.GetByEmail(email));
            return ApplicationMapper.Mapper.Map<UserModel>(users.SingleOrDefault());
        }
        catch (Exception e)
        {
            throw ApplicationExceptionsHandlingHelper.HandleRetrieveDataException(e, _logger);
        }
    }

    public async Task<int> CountAsync()
    {
        try
        {
            return await _usersRepository.CountAsync();
        }
        catch (Exception e)
        {
            throw ApplicationExceptionsHandlingHelper.HandleRetrieveDataException(e, _logger);
        }
    }

    public async Task<bool> VerifyPasswordAsync(UserModel user, string password)
    {
        try
        {
            var userEntity = await _usersRepository.GetByIdAsync(user.Id);
            if (userEntity == null)
                return false;

            return VerifyPassword(userEntity, password);
        }
        catch (Exception e)
        {
            throw ApplicationExceptionsHandlingHelper.HandleMutateDataException(e, "An error occurred while verifying password", _logger);
        }
        
    }

    private bool VerifyPassword(User user, string password)
    {
        return _passwordHasher.VerifyPassword(user.PasswordHash, password);
    }

    public async Task<OperationResult<UserModel>> CreateAsync(UserModel userModel)
    {
        try
        {
            var validationResult = await _userValidator.ValidateAsync(userModel);
            if (!validationResult.IsValid)
                return OperationResultExtensions.ValidationErrorResult<UserModel>(validationResult);
        
            var userEntity = ApplicationMapper.Mapper.Map<User>(userModel);
            userEntity.PasswordHash = string.Empty;

            var alreadyExistsResult = await ValidateAlreadyExists(userEntity);
            if (alreadyExistsResult != null) return alreadyExistsResult;
        
            var result = await _usersRepository.CreateAsync(userEntity);
            return OperationResultExtensions.OkResult(ApplicationMapper.Mapper.Map<UserModel>(result));
        }
        catch (Exception)
        {
            return OperationResultExtensions.ErrorResult<UserModel>("An error occurred while creating the user");
        }
    }
    
    public async Task<OperationResult<UserModel>> CreateAsync(UserModel userModel, string password)
    {
        try
        {
            ArgumentException.ThrowIfNullOrEmpty(password);
            var passwordValidationResult = await _passwordValidator.ValidateAsync(new Password(password));
            var validationResult = await _userValidator.ValidateAsync(userModel);

            if (!validationResult.IsValid || !passwordValidationResult.IsValid)
            {
                if (validationResult.IsValid)
                    return OperationResultExtensions.ValidationErrorResult<UserModel>(new Dictionary<string, string[]>{{nameof(Password),passwordValidationResult.Errors.Select(e => e.ErrorMessage).ToArray()}});
                
                if (passwordValidationResult.IsValid)
                    return OperationResultExtensions.ValidationErrorResult<UserModel>(validationResult);

                var validationErrors = validationResult.ToDictionary();
                validationErrors.Add(nameof(Password), passwordValidationResult.Errors.Select(e => e.ErrorMessage).ToArray());
            }
            
            var userEntity = ApplicationMapper.Mapper.Map<User>(userModel);
            userEntity.PasswordHash = _passwordHasher.HashPassword(password);

            var alreadyExistsResult = await ValidateAlreadyExists(userEntity);
            if (alreadyExistsResult != null) return alreadyExistsResult;
        
            var result = await _usersRepository.CreateAsync(userEntity);
            return OperationResultExtensions.OkResult(ApplicationMapper.Mapper.Map<UserModel>(result));
        }
        catch (Exception)
        {
            return OperationResultExtensions.ErrorResult<UserModel>("An error occurred while creating the user");
        }
    }

    private async Task<OperationResult<UserModel>?> ValidateAlreadyExists(User user)
    {
        var usersByName = await _usersRepository.GetAsync(UserSpecification.GetByName(user.UserName));
        if (usersByName.Any())
            return OperationResultExtensions.ValidationErrorResult<UserModel>(
                new Dictionary<string, string[]>
                {
                    { nameof(Book.Title), ["User with this name already exists"] }, 
                });

        if (!string.IsNullOrEmpty(user.Email))
        {
            var usersByEmail = await _usersRepository.GetAsync(UserSpecification.GetByEmail(user.Email));
            if (usersByEmail.Any())
                return OperationResultExtensions.ValidationErrorResult<UserModel>(
                    new Dictionary<string, string[]>
                    {
                        { nameof(Book.Title), ["User with this email already exists"] }, 
                    });
        }
        
        return null;
    }

    public async Task<OperationResult<UserModel>> UpdateAsync(UserModel userModel)
    {
        try
        {
            var validationResult = await _userValidator.ValidateAsync(userModel);
            if (!validationResult.IsValid)
                return OperationResultExtensions.ValidationErrorResult<UserModel>(validationResult);
            
            var userEntity = await _usersRepository.GetByIdAsync(userModel.Id);
            if (userEntity == null) return OperationResultExtensions.ErrorResult<UserModel>("User was not found", 404);

            var alreadyExistsResult = await ValidateAlreadyExists(userEntity);
            if (alreadyExistsResult != null) return alreadyExistsResult;
        
            var result = await _usersRepository.UpdateAsync(userEntity);
        
            return OperationResultExtensions.OkResult(ApplicationMapper.Mapper.Map<UserModel>(result));
        }
        catch (Exception)
        {
            return OperationResultExtensions.ErrorResult<UserModel>("An error occurred while updating the user");
        }
    }

    public async Task<OperationResult<UserModel>> UpdateUserPassword(UserModel user, string currentPassword, string newPassword)
    {
        try
        {
            var validationResult = await _passwordValidator.ValidateAsync(new Password(newPassword));
            if (!validationResult.IsValid)
            {
                return OperationResultExtensions.ValidationErrorResult<UserModel>(new Dictionary<string, string[]>
                {
                    {nameof(Password), validationResult.Errors.Where(e => e.PropertyName == nameof(Password)).Select(e => e.ErrorMessage).ToArray()}
                });
            }
            
            var userEntity = await _usersRepository.GetByIdAsync(user.Id);
            if (userEntity == null) return OperationResultExtensions.ErrorResult<UserModel>("User was not found", 404);

            if (!VerifyPassword(userEntity, currentPassword))
                return OperationResultExtensions.ValidationErrorResult<UserModel>(new Dictionary<string, string[]>
                {
                    { "OldPassword", ["Wrong password"] }
                });
        
            userEntity.PasswordHash = _passwordHasher.HashPassword(newPassword);
        
            var result = await _usersRepository.UpdateAsync(userEntity);
            return OperationResultExtensions.OkResult(ApplicationMapper.Mapper.Map<UserModel>(result));
        }
        catch (Exception)
        {
            return OperationResultExtensions.ErrorResult<UserModel>("An error occurred while updating the password");
        }
    }

    public async Task<OperationResult<UserModel>> DeleteAsync(UserModel user)
    {
        try
        {
            var userEntity = await _usersRepository.GetByIdAsync(user.Id);
            if (userEntity == null) 
                return OperationResultExtensions.ErrorResult<UserModel>("User was not found", 404);
        
            var result = await _usersRepository.DeleteAsync(userEntity);
            return OperationResultExtensions.OkResult(ApplicationMapper.Mapper.Map<UserModel>(result));
        }
        catch (Exception)
        {
            return OperationResultExtensions.ErrorResult<UserModel>("An error occurred while deleting the user");
        }
    }
}