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
public class UsersController : ApiController
{
    private readonly IUsersService _usersService;
    private readonly IMapper _mapper;


    public UsersController(IUsersService usersService, IMapper mapper)
    {
        _usersService = usersService;
        _mapper = mapper;
    }

    // todo get users list
    
    [Authorize] // todo view permissions
    [HttpGet("users/{userId:guid}")]
    public async Task<IActionResult> Profile(Guid userId)
    {
        var user = await _usersService.GetByIdAsync(userId);
        if (user == null)
            return Problem(detail: "User was not found", statusCode: 404);
        
        return Ok(_mapper.Map<UserProfileViewModel>(user));
    }

    [AuthorizeAsCurrentUser("userId")]
    [HttpPut("users/{userId:guid}")]
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
    [HttpPatch("users/{userId:guid}/password")]
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
    [HttpDelete("users/{userId:guid}")]
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