namespace UsersAdministration.Dtos.UserDtos.UpdateDtos;

public class UserUpdateLoginDto
{
    public required string UserOldLogin { get; set; }
    
    public required string NewLogin { get; set; }
}