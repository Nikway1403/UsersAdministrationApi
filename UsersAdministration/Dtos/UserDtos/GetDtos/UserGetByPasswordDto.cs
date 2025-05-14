namespace UsersAdministration.Dtos.UserDtos.GetDtos;

public class UserGetByPasswordDto
{
    public required string Login { get; set; }

    public required string Password { get; set; }
}