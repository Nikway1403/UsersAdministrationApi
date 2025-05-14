namespace UsersAdministration.Dtos.UserDtos.UpdateDtos;

public class UserUpdateBirthdayDto
{
    public required string UserLogin { get; set; }
    
    public required DateTime NewDate { get; set; }
}