namespace UsersAdministration.Dtos.UserDtos.UpdateDtos;

public class UserUpdatePasswordDto
{
    public required string UserLogin { get; set; }
    
    public required string NewPassword { get; set; }
}