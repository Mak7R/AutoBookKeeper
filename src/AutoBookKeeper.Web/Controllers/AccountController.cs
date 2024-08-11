using System.Security.Claims;
using AutoBookKeeper.Application.Interfaces;
using AutoBookKeeper.Application.Models;
using AutoBookKeeper.Web.Controllers.Base;
using AutoBookKeeper.Web.Models;
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

    [HttpPost("account/register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var user = _mapper.Map<UserModel>(registerDto);
        var result = await _usersService.CreateUserAsync(user, registerDto.Password);

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
    
    [HttpPost("account/login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var user = await _usersService.GetByNameAsync(loginDto.Name);
        if (user == null)
            return Problem(detail: "User was not found", statusCode: 404);

        var isCorrectPassword = await _usersService.VerifyPasswordAsync(user, loginDto.Password);

        if (!isCorrectPassword)
            return Problem("Invalid email or password", statusCode: 400);
        
        var token = _authenticationService.GenerateToken(user);

        return Ok(new AuthenticationResponse
        {
            Token = token,
            User = _mapper.Map<UserViewModel>(user)
        });
    }

    [Authorize]
    [HttpGet("account/{userId:guid}")]
    public async Task<IActionResult> Profile(Guid userId)
    {
        var user = await _usersService.GetByIdAsync(userId);
        if (user == null)
            return Problem(detail: "User was not found", statusCode: 404);

        return Ok(_mapper.Map<UserProfileViewModel>(user));
    }

    [Authorize]
    [HttpDelete("account/{userId:guid}")]
    public async Task<IActionResult> Delete(Guid userId, DeleteAccountRequestDto deleteRequestDto)
    {
        var user = await _usersService.GetByIdAsync(userId);
        if (user == null)
            return Problem(detail: "User was not found", statusCode: 404);
        
        var currentUserIdString = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        
        if (!Guid.TryParse(currentUserIdString, out var currentUserId))
            return Problem($"Current user has not permission to delete user with id {userId}", statusCode:StatusCodes.Status403Forbidden);

        if (currentUserId != userId)
            return Problem($"Current user has not permission to delete user with id {userId}", statusCode:StatusCodes.Status403Forbidden);
        
        if (await _usersService.VerifyPasswordAsync(user, deleteRequestDto.Password))
            return Problem($"Current user has not permission to delete user with id {userId}", statusCode:StatusCodes.Status403Forbidden);
        
        var result = await _usersService.DeleteUserAsync(user);

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