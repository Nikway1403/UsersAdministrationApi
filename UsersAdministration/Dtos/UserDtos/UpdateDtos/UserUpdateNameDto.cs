namespace UsersAdministration.Dtos.UserDtos.UpdateDtos;

public class UserUpdateNameDto
{
    public required string UserLogin { get; set; }
    
    public required string NewName { get; set; }
}