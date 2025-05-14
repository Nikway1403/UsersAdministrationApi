using System.ComponentModel.DataAnnotations;

namespace UsersAdministration.Dtos.UserDtos;

public class UserCreationDto
{
    [Required]
    [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Login must be alphanumeric")]
    public required string Login { get; set; }
    
    [Required]
    [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Password must be alphanumeric")]
    public required string Password { get; set; }
    
    [Required]
    [RegularExpression("^[a-zA-Zа-яА-ЯёЁ]+$", ErrorMessage = "Name must be letters only")]
    public required string Name { get; set; }
    
    [Range(0, 2)]
    public int Gender { get; set; }
    
    public DateTime? Birthday { get; set; }
    
    public bool IsAdmin { get; set; }
}