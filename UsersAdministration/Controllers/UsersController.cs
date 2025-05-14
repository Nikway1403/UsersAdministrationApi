using Microsoft.AspNetCore.Mvc;
using UsersAdministration.Dtos.UserDtos;
using UsersAdministration.Dtos.UserDtos.DeleteDtos;
using UsersAdministration.Dtos.UserDtos.GetDtos;
using UsersAdministration.Dtos.UserDtos.UpdateDtos;
using UsersAdministration.Services.Users;

namespace UsersAdministration.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private IUserService _userService;
    
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
    
    /// <summary>
    /// Create a new user
    /// </summary>
    /// <response code="200">Successfully created</response>
    /// <response code="400">Validation failed or login not unique</response>
    /// <response code="403">Only admin can create another admin</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateUser(
        [FromBody] UserCreationDto dto, 
        [FromQuery] string createdBy)
    {
        await _userService.CreateUser(dto, createdBy);
        return Ok();
    }
    
    /// <summary>
    /// Update user name
    /// </summary>
    /// <response code="200">Successfully updated</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">User not found</response>
    [HttpPut("name")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeName(
        [FromBody] UserUpdateNameDto dto,
        [FromQuery] string userBy)
    {
        await _userService.UpdateName(dto, userBy);
        return Ok();
    }
    
    /// <summary>
    /// Update user birthday
    /// </summary>
    [HttpPut("birthday")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeBirthday(
        [FromBody] UserUpdateBirthdayDto dto, 
        [FromQuery] string userBy)
    {
        await _userService.UpdateBirthday(dto, userBy);
        return Ok();
    }
    
    /// <summary>
    /// Update user gender
    /// </summary>
    [HttpPut("gender")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeGender(
        [FromBody] UserUpdateGenderDto dto,
        [FromQuery] string userBy)
    {
        await _userService.UpdateGender(dto, userBy);
        return Ok();
    }
    
    /// <summary>
    /// Update user password
    /// </summary>
    [HttpPut("password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangePassword(
        [FromBody] UserUpdatePasswordDto dto,
        [FromQuery] string userBy)
    {
        await _userService.UpdatePassword(dto, userBy);
        return Ok();
    }
    
    /// <summary>
    /// Update user login
    /// </summary>
    [HttpPut("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeLogin(
        [FromBody] UserUpdateLoginDto dto,
        [FromQuery] string userBy)
    {
        await _userService.UpdateLogin(dto, userBy);
        return Ok();
    }

    /// <summary>
    /// Get all active users
    /// </summary>
    [HttpGet("active")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetActiveUsers(
        [FromQuery] string adminLogin)
    {
        return Ok(await _userService.GetActiveUsers(adminLogin));
    }

    /// <summary>
    /// Get user by login
    /// </summary>
    [HttpGet("login/{userLogin}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserByLogin(
        [FromRoute] string userLogin,
        [FromQuery] string userBy)
    {
        return Ok(await _userService.GetUserByLogin(userLogin, userBy));
    }

    /// <summary>
    /// Authenticate user by login and password
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserByLoginAndPassword(
        [FromBody] UserGetByPasswordDto dto)
    {
        return Ok(await _userService.GetUserByLoginAndPassword(dto));
    }

    /// <summary>
    /// Get users older than specified age
    /// </summary>
    [HttpGet("age/{age}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetUserOlderThan(
        [FromRoute] int age, 
        [FromQuery] string userBy)
    {
        return Ok(await _userService.GetUsersOlderThan(age, userBy));
    }

    /// <summary>
    /// Soft delete of user
    /// </summary>
    [HttpDelete("soft")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UserSoftDelete(
        [FromBody] UserDeleteDto dto,
        [FromQuery] string userBy)
    {
        await _userService.SoftDeleteUser(dto, userBy);
        return NoContent();
    }

    /// <summary>
    /// Hard delete of user
    /// </summary>
    [HttpDelete("hard")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UserHardDelete(
        [FromBody] UserDeleteDto dto,
        [FromQuery] string userBy)
    {
        await _userService.HardDeleteUser(dto, userBy);
        return NoContent();
    }

    /// <summary>
    /// Restore previously soft-deleted user
    /// </summary>
    [HttpPut("restore")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UserRestore(
        [FromBody] UserDeleteDto dto,
        [FromQuery] string userBy)
    {
        await _userService.RestoreUser(dto, userBy);
        return Ok();
    }
}