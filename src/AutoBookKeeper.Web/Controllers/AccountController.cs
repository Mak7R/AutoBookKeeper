using AutoBookKeeper.Application.Interfaces;
using AutoBookKeeper.Application.Models;
using AutoBookKeeper.Web.Controllers.Base;
using AutoBookKeeper.Web.Models.Account;
using AutoBookKeeper.Web.Models.User;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoBookKeeper.Web.Controllers;

[AllowAnonymous]
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
    
    [HttpPost("/register")]
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
    
    [HttpPost("/login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var user = await _usersService.GetByUserNameAsync(loginDto.UserName);
        if (user == null)
            return Problem(detail: "User was not found", statusCode: 404);

        if (!await _usersService.VerifyPasswordAsync(user, loginDto.Password))
            return Problem("Invalid email or password", statusCode: 400);
        
        var result = await _authenticationService.GenerateTokenAsync(user);

        if (!result.HasValue)
            return Problem("Error was occured while generating access token", statusCode: 500);
        
        return Ok(new AuthenticationResponse
        {
            UserId = user.Id,
            AccessToken = result.Value.AccessToken,
            RefreshToken = result.Value.RefreshToken
        });
    }

    [HttpPost("/refresh-token")]
    public async Task<IActionResult> RefreshToken(RefreshTokenDto refreshTokenDto)
    {
        var user = await _usersService.GetByIdAsync(refreshTokenDto.UserId);
        if (user == null) return Problem("User was not found", statusCode: 404);

        var result = await _authenticationService.RefreshAccessTokenAsync(user, refreshTokenDto.RefreshToken);

        if (!result.HasValue)
            return Unauthorized();
        
        return Ok(new AuthenticationResponse
        {
            UserId = user.Id,
            AccessToken = result.Value.AccessToken,
            RefreshToken = result.Value.RefreshToken
        });
    }
}