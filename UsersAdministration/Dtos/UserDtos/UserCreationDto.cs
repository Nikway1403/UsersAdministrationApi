namespace UsersAdministration.Dtos.UserDtos;

public class UserCreationDto
{
    public required string Login { get; set; }
    
    public required string Password { get; set; }
    
    public required string Name { get; set; }
    
    public int Gender { get; set; }
    
    public DateTime? Birthday { get; set; }
    
    public bool IsAdmin { get; set; }
}