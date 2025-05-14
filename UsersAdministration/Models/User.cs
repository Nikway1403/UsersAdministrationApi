namespace UsersAdministration.Models;

public class User
{
    public Guid Guid { get; set; }
    
    public required string Login { get; set; }
    
    public required string Password { get; set; }
    
    public required string Name { get; set; }

    public int Gender { get; set; }

    public DateTime? Birthday { get; set; }

    public bool IsAdmin { get; init; }

    public DateTime CreatedOn { get; init; } = DateTime.UtcNow;

    public required string CreatedBy { get; set; }
    
    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? RevokedOn { get; set; }

    public string? RevokedBy { get; set; }
}
