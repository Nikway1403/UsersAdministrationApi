using Microsoft.AspNetCore.Mvc;
using UsersAdministration.Dtos.UserDtos;
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
    
    [HttpPost]
    public IActionResult CreateUser([FromBody] UserCreationDto dto, [FromQuery] string createdBy)
    {
        if (!_userService.CreateUser(dto, createdBy))
            return BadRequest();

        return Ok();
    }
}