namespace UsersAdministration.Dtos.UserDtos.UpdateDtos;

public class UserUpdateGenderDto
{
    public required string UserLogin { get; set; }
    
    public int NewGender { get; set; }
}