namespace UsersAdministration.Dtos.UserDtos.ReturnDtos;

public class UserReturnDto
{
    public required string Name { get; set; }

    public int Gender { get; set; }

    public DateTime? Birthday { get; set; }

    public bool Active { get; set; }
}