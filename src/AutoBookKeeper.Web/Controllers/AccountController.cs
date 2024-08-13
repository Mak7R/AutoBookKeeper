using AutoBookKeeper.Application.Interfaces;
using AutoBookKeeper.Application.Models;
using AutoBookKeeper.Web.Controllers.Base;
using AutoBookKeeper.Web.Filters;
using AutoBookKeeper.Web.Models.User;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoBookKeeper.Web.Controllers;

[ApiVersion("1.0")]
public class AccountController : ApiController
{
    private readonly IUsersService _usersService;
    private readonly IAuthenticationService _authenticationService;
    private readonly IMapper _mapper;

    public AccountController(IUsersService usersService, IAuthenticationService authenticationService, IMapper mapper)
    {
        _usersService = usersService;
        _authenticationService = authenticationService;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpPost("account/register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var user = _mapper.Map<UserModel>(registerDto);
        var result = await _usersService.CreateAsync(user, registerDto.Password);

        if (result.IsSuccessful)
            return Ok(_mapper.Map<UserViewModel>(result.Result));
        
        return StatusCode(result.Status, new ProblemDetails
        {
            Detail = "Registration was not successful",
            Status = result.Status,
            Extensions = new Dictionary<string, object?>
            {
                { "errors", result.Errors }
            }
        });
    }
    
    [AllowAnonymous]
    [HttpPost("account/login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var user = await _usersService.GetByUserNameAsync(loginDto.UserName);
        if (user == null)
            return Problem(detail: "User was not found", statusCode: 404);

        if (!await _usersService.VerifyPasswordAsync(user, loginDto.Password))
            return Problem("Invalid email or password", statusCode: 400);
        
        var token = _authenticationService.GenerateToken(user);

        return Ok(new AuthenticationResponse
        {
            Token = token,
            User = _mapper.Map<UserProfileViewModel>(user)
        });
    }

    [Authorize] // todo view permissions
    [HttpGet("account/{userId:guid}")]
    public async Task<IActionResult> Profile(Guid userId)
    {
        var user = await _usersService.GetByIdAsync(userId);
        if (user == null)
            return Problem(detail: "User was not found", statusCode: 404);
        
        return Ok(_mapper.Map<UserProfileViewModel>(user));
    }

    [AuthorizeAsCurrentUser("userId")]
    [HttpPut("account/{userId:guid}")]
    public async Task<IActionResult> Update(Guid userId, UpdateUserDto updateUserDto)
    {
        var user = await _usersService.GetByIdAsync(userId);
        if (user == null)
            return Problem(detail: "User was not found", statusCode: 404);
        
        if (!await _usersService.VerifyPasswordAsync(user, updateUserDto.CurrentPassword))
            return Problem($"Current user has not permission to update user with id {userId}", statusCode:StatusCodes.Status403Forbidden);

        var updateUser = _mapper.Map<UserModel>(updateUserDto);
        updateUser.Id = userId;
        
        var result = await _usersService.UpdateAsync(updateUser);
        
        if (result.IsSuccessful)
            return Ok(_mapper.Map<UserViewModel>(result.Result));
        
        return StatusCode(result.Status, new ProblemDetails
        {
            Detail = "Update was not successful",
            Status = result.Status,
            Extensions = new Dictionary<string, object?>
            {
                { "errors", result.Errors }
            }
        });
    }

    [AuthorizeAsCurrentUser("userId")]
    [HttpPatch("account/{userId:guid}/password")]
    public async Task<IActionResult> UpdatePassword(Guid userId, UpdateUserPasswordDto userPasswordDto)
    {
        var user = await _usersService.GetByIdAsync(userId);
        if (user == null)
            return Problem(detail: "User was not found", statusCode: 404);

        var result =
            await _usersService.UpdateUserPassword(user, userPasswordDto.CurrentPassword,
                userPasswordDto.NewPassword);
        
        if (result.IsSuccessful)
            return Ok(_mapper.Map<UserViewModel>(result.Result));
        
        return StatusCode(result.Status, new ProblemDetails
        {
            Detail = "Deletion was not successful",
            Status = result.Status,
            Extensions = new Dictionary<string, object?>
            {
                { "errors", result.Errors }
            }
        });
    }

    [AuthorizeAsCurrentUser("userId")]
    [HttpDelete("account/{userId:guid}")]
    public async Task<IActionResult> Delete(Guid userId, DeleteAccountRequestDto deleteRequestDto)
    {
        var user = await _usersService.GetByIdAsync(userId);
        if (user == null)
            return Problem(detail: "User was not found", statusCode: 404);
        
        if (!await _usersService.VerifyPasswordAsync(user, deleteRequestDto.Password))
            return Problem($"Current user has not permission to delete user with id {userId}", statusCode:StatusCodes.Status403Forbidden);
        
        var result = await _usersService.DeleteAsync(user);

        if (result.IsSuccessful)
            return Ok(_mapper.Map<UserViewModel>(result.Result));
        
        return StatusCode(result.Status, new ProblemDetails
        {
            Detail = "Deletion was not successful",
            Status = result.Status,
            Extensions = new Dictionary<string, object?>
            {
                { "errors", result.Errors }
            }
        });
    }
}